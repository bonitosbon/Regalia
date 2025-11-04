using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Security;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Regalia_Front_End.Models;

namespace Regalia_Front_End.Services
{
    public class ApiService : IDisposable
    {
        private static readonly string BaseUrl = GetBackendUrl() + "/api/"; // Backend URL from config
        private HttpClient _httpClient;

        private static string GetBackendUrl()
        {
            try
            {
                string url = System.Configuration.ConfigurationManager.AppSettings["BackendUrl"];
                if (!string.IsNullOrEmpty(url))
                {
                    // Remove trailing slash if present
                    return url.TrimEnd('/');
                }
            }
            catch
            {
                // Fallback if config fails
            }
            // Default fallback
            return "http://localhost:7288";
        }
        private static string _currentToken = string.Empty;

        static ApiService()
        {
            // Bypass SSL certificate validation for localhost in development
            // WARNING: Only use this in development, never in production!
            ServicePointManager.ServerCertificateValidationCallback =
                delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                {
                    // For localhost development, accept all certificates
                    // In production, you should validate certificates properly
                    return true; // Accept all certificates for development
                };
        }

        public ApiService()
        {
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(60); // 60 seconds timeout for Render free tier wake-up
            _httpClient.BaseAddress = new Uri(BaseUrl);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            // Set token if available
            if (!string.IsNullOrEmpty(_currentToken))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _currentToken);
            }
        }

        public static string CurrentToken => _currentToken;

        /// <summary>
        /// Logs out the current user by clearing the authentication token.
        /// </summary>
        public static void Logout()
        {
            _currentToken = string.Empty;
            System.Diagnostics.Debug.WriteLine("User logged out - token cleared");
        }

        /// <summary>
        /// Attempts to login via API. Returns LoginResponse on success, null on failure.
        /// </summary>
        public async Task<LoginResponse> LoginAsync(LoginDto loginDto)
        {
            try
            {
                // Serialize login data to JSON
                string json = SimpleJsonSerializer.Serialize(loginDto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Make POST request to login endpoint
                HttpResponseMessage response = await _httpClient.PostAsync("auth/login", content);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine($"Login API response: {responseContent}");

                    LoginResponse loginResponse = SimpleJsonSerializer.Deserialize(responseContent);

                    if (loginResponse != null && !string.IsNullOrEmpty(loginResponse.Token))
                    {
                        _currentToken = loginResponse.Token;
                        _httpClient.DefaultRequestHeaders.Authorization =
                            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _currentToken);

                        System.Diagnostics.Debug.WriteLine($"Token extracted successfully. Length: {_currentToken.Length}");
                        return loginResponse;
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"Failed to extract token. Response was null or token was empty.");
                        System.Diagnostics.Debug.WriteLine($"Response content: {responseContent}");
                    }
                }
                else
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine($"Login failed: {response.StatusCode} - {errorContent}");

                    // Throw exception with error message so caller can show it
                    string errorMessage = "Invalid username/email or password.";
                    if (errorContent.Contains("Invalid login attempt"))
                    {
                        errorMessage = "Invalid username/email or password. Please check your credentials and try again.";
                    }
                    else if (!string.IsNullOrEmpty(errorContent) && errorContent.Length < 200)
                    {
                        // Use the error message from backend if it's reasonable
                        errorMessage = errorContent.Trim().Trim('"');
                    }

                    throw new Exception(errorMessage);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error during API login: {ex.Message}");
                // Return null to indicate failure - will fallback to hardcoded login
            }

            return null;
        }

        /// <summary>
        /// Attempts to register a new user via API. Returns true on success, false on failure.
        /// </summary>
        public async Task<bool> RegisterAsync(RegisterDto registerDto)
        {
            try
            {
                // Serialize registration data to JSON
                string json = SimpleJsonSerializer.Serialize(registerDto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Make POST request to register endpoint
                HttpResponseMessage response = await _httpClient.PostAsync("auth/register", content);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine($"Registration successful: {responseContent}");

                    // Double-check - sometimes Identity returns success but with warnings
                    if (responseContent.Contains("errors") || responseContent.Contains("Errors"))
                    {
                        System.Diagnostics.Debug.WriteLine("WARNING: Registration response contains error indicators!");
                        string errorMsg = ExtractErrorMessage(responseContent);
                        throw new Exception(errorMsg);
                    }

                    return true;
                }
                else
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine($"Registration failed: {response.StatusCode} - {errorContent}");

                    // Try to extract more readable error message from the response
                    string errorMessage = ExtractErrorMessage(errorContent);
                    throw new Exception(errorMessage);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error during API registration: {ex.Message}");
                throw; // Re-throw so caller can handle the error message
            }
        }

        /// <summary>
        /// Creates a new condo property via API. Returns true on success, throws exception on failure.
        /// </summary>
        public async Task<bool> CreateCondoAsync(CreateCondoDto condoDto)
        {
            try
            {
                // Ensure we have a token
                if (string.IsNullOrEmpty(_currentToken))
                {
                    throw new Exception("You must be logged in to create a property. Please log in first.");
                }

                // Serialize condo data to JSON
                string json = SimpleJsonSerializer.Serialize(condoDto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Make POST request to create condo endpoint
                HttpResponseMessage response = await _httpClient.PostAsync("condo/create", content);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine($"Condo created successfully: {responseContent}");
                    return true;
                }
                else
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine($"Condo creation failed: {response.StatusCode} - {errorContent}");

                    string errorMessage = ExtractErrorMessage(errorContent);
                    throw new Exception(errorMessage);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error during API condo creation: {ex.Message}");
                throw; // Re-throw so caller can handle the error message
            }
        }

        /// <summary>
        /// Gets all condos owned by the logged-in owner from the API.
        /// </summary>
        public async Task<List<CondoResponse>> GetOwnerCondosAsync()
        {
            try
            {
                // Ensure we have a token
                if (string.IsNullOrEmpty(_currentToken))
                {
                    throw new Exception("You must be logged in to view properties. Please log in first.");
                }

                // Make GET request to get owner condos endpoint
                HttpResponseMessage response = await _httpClient.GetAsync("condo/owner");

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine($"GetOwnerCondos response: {responseContent}");

                    // Parse JSON array response
                    List<CondoResponse> condos = SimpleJsonSerializer.DeserializeCondoList(responseContent);
                    return condos ?? new List<CondoResponse>();
                }
                else
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine($"GetOwnerCondos failed: {response.StatusCode} - {errorContent}");

                    string errorMessage = ExtractErrorMessage(errorContent);
                    throw new Exception(errorMessage);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error during API get owner condos: {ex.Message}");
                throw; // Re-throw so caller can handle the error message
            }
        }

        /// <summary>
        /// Updates an existing condo property via API. Returns true on success, throws exception on failure.
        /// </summary>
        public async Task<bool> UpdateCondoAsync(int condoId, UpdateCondoDto condoDto)
        {
            try
            {
                // Ensure we have a token
                if (string.IsNullOrEmpty(_currentToken))
                {
                    throw new Exception("You must be logged in to update a property. Please log in first.");
                }

                // Serialize condo data to JSON
                string json = SimpleJsonSerializer.Serialize(condoDto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Make PUT request to update condo endpoint
                HttpResponseMessage response = await _httpClient.PutAsync($"condo/{condoId}", content);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine($"Condo updated successfully: {responseContent}");
                    return true;
                }
                else
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine($"Condo update failed: {response.StatusCode} - {errorContent}");

                    string errorMessage = ExtractErrorMessage(errorContent);
                    throw new Exception(errorMessage);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error during API condo update: {ex.Message}");
                throw; // Re-throw so caller can handle the error message
            }
        }

        /// <summary>
        /// Updates condo status via API. Returns true on success, throws exception on failure.
        /// </summary>
        public async Task<bool> UpdateCondoStatusAsync(int condoId, string status)
        {
            try
            {
                // Ensure we have a token
                if (string.IsNullOrEmpty(_currentToken))
                {
                    throw new Exception("You must be logged in to update a property status. Please log in first.");
                }

                // Create status DTO
                var statusDto = new { Status = status };
                string json = SimpleJsonSerializer.Serialize(statusDto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Make PATCH request to update condo status endpoint (HttpClient doesn't have PatchAsync in .NET Framework)
                var request = new HttpRequestMessage(new HttpMethod("PATCH"), $"condo/{condoId}/status")
                {
                    Content = content
                };
                HttpResponseMessage response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine($"Condo status updated successfully: {responseContent}");
                    return true;
                }
                else
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine($"Condo status update failed: {response.StatusCode} - {errorContent}");

                    string errorMessage = ExtractErrorMessage(errorContent);
                    throw new Exception(errorMessage);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error during API condo status update: {ex.Message}");
                throw; // Re-throw so caller can handle the error message
            }
        }

        /// <summary>
        /// Deletes a condo property via API. Returns true on success, throws exception on failure.
        /// </summary>
        public async Task<bool> DeleteCondoAsync(int condoId)
        {
            try
            {
                // Ensure we have a token
                if (string.IsNullOrEmpty(_currentToken))
                {
                    throw new Exception("You must be logged in to delete a property. Please log in first.");
                }

                // Construct the full URL for debugging
                string fullUrl = $"{BaseUrl}condo/{condoId}";
                System.Diagnostics.Debug.WriteLine($"DeleteCondoAsync: Making DELETE request to: {fullUrl}");
                System.Diagnostics.Debug.WriteLine($"DeleteCondoAsync: Token present: {!string.IsNullOrEmpty(_currentToken)}");
                System.Diagnostics.Debug.WriteLine($"DeleteCondoAsync: Token length: {(_currentToken?.Length ?? 0)}");

                // Make DELETE request to delete condo endpoint using HttpRequestMessage for explicit control
                // Try lowercase first (matches other endpoints), then fallback to capitalized if needed
                var request = new HttpRequestMessage(HttpMethod.Delete, $"condo/{condoId}");

                // Ensure Authorization header is set (in case it wasn't set in constructor)
                if (!string.IsNullOrEmpty(_currentToken) && !request.Headers.Contains("Authorization"))
                {
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _currentToken);
                }

                HttpResponseMessage response = await _httpClient.SendAsync(request);

                // If we get MethodNotAllowed, try with capitalized controller name
                if (response.StatusCode == HttpStatusCode.MethodNotAllowed)
                {
                    System.Diagnostics.Debug.WriteLine($"DeleteCondoAsync: First attempt failed, trying with capitalized 'Condo'");
                    request = new HttpRequestMessage(HttpMethod.Delete, $"Condo/{condoId}");
                    if (!string.IsNullOrEmpty(_currentToken) && !request.Headers.Contains("Authorization"))
                    {
                        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _currentToken);
                    }
                    response = await _httpClient.SendAsync(request);
                }

                System.Diagnostics.Debug.WriteLine($"DeleteCondoAsync: Response status: {response.StatusCode}");

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine($"Condo deleted successfully: {responseContent}");
                    return true;
                }
                else
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine($"Condo deletion failed: {response.StatusCode} - {errorContent}");

                    // Include more details in error message for MethodNotAllowed
                    string errorMessage = ExtractErrorMessage(errorContent);
                    if (response.StatusCode == HttpStatusCode.MethodNotAllowed)
                    {
                        errorMessage = $"Method not allowed. The DELETE endpoint may not be configured correctly on the server. Status: {response.StatusCode}, URL: {fullUrl}";
                    }
                    throw new Exception(errorMessage);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error during API condo deletion: {ex.Message}");
                throw; // Re-throw so caller can handle the error message
            }
        }

        /// <summary>
        /// Extracts a readable error message from API error responses.
        /// </summary>
        private string ExtractErrorMessage(string errorContent)
        {
            try
            {
                // Common Identity error messages
                if (errorContent.Contains("Passwords must be at least"))
                {
                    return "Password must be at least 6 characters long and contain at least one uppercase letter, one lowercase letter, and one number.";
                }
                if (errorContent.Contains("Passwords must have at least one non alphanumeric character"))
                {
                    return "Password must contain at least one special character (e.g., !@#$%^&*).";
                }
                if (errorContent.Contains("Passwords must have at least one digit"))
                {
                    return "Password must contain at least one number.";
                }
                if (errorContent.Contains("Passwords must have at least one uppercase"))
                {
                    return "Password must contain at least one uppercase letter.";
                }
                if (errorContent.Contains("Passwords must have at least one lowercase"))
                {
                    return "Password must contain at least one lowercase letter.";
                }
                if (errorContent.Contains("Email is already in use"))
                {
                    return "This email address is already registered.";
                }
                if (errorContent.Contains("Username is already in use"))
                {
                    return "This username is already taken.";
                }

                // Try to extract message from JSON format like: "Email is already in use."
                if (errorContent.Contains("\""))
                {
                    int start = errorContent.IndexOf("\"");
                    int end = errorContent.LastIndexOf("\"");
                    if (end > start && start >= 0)
                    {
                        string extracted = errorContent.Substring(start + 1, end - start - 1);
                        if (!string.IsNullOrWhiteSpace(extracted) && extracted.Length < 200)
                        {
                            return extracted;
                        }
                    }
                }

                // Fallback to raw content (trimmed)
                return errorContent.Trim().Trim('"').Trim();
            }
            catch
            {
                return errorContent;
            }
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }

        // Get all bookings for the owner
        public async Task<List<BookingResponse>> GetOwnerBookingsAsync()
        {
            try
            {
                // Ensure we have a token
                if (string.IsNullOrEmpty(_currentToken))
                {
                    throw new Exception("You must be logged in to view bookings. Please log in first.");
                }

                var request = new HttpRequestMessage(HttpMethod.Get, BaseUrl + "Booking/owner");
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _currentToken);

                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                string json = await response.Content.ReadAsStringAsync();
                return DeserializeBookings(json);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting owner bookings: {ex.Message}");
                return new List<BookingResponse>();
            }
        }

        // Get all bookings for the front desk
        public async Task<List<BookingResponse>> GetFrontDeskBookingsAsync()
        {
            try
            {
                // Ensure we have a token
                if (string.IsNullOrEmpty(_currentToken))
                {
                    throw new Exception("You must be logged in to view bookings. Please log in first.");
                }

                var request = new HttpRequestMessage(HttpMethod.Get, BaseUrl + "Booking/frontdesk");
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _currentToken);

                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                string json = await response.Content.ReadAsStringAsync();
                return DeserializeBookings(json);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting front desk bookings: {ex.Message}");
                return new List<BookingResponse>();
            }
        }

        // Check-in guest using QR code
        public async Task<bool> CheckInGuestAsync(int bookingId, string qrCodeData)
        {
            try
            {
                // Ensure we have a token
                if (string.IsNullOrEmpty(_currentToken))
                {
                    throw new Exception("You must be logged in to check in guests. Please log in first.");
                }

                // Validate QR code data
                if (string.IsNullOrEmpty(qrCodeData))
                {
                    throw new Exception("QR code data is required for check-in.");
                }

                // Use PascalCase to match backend DTO
                var requestData = new
                {
                    QrCodeData = qrCodeData
                };

                var json = SimpleJsonSerializer.Serialize(requestData);
                System.Diagnostics.Debug.WriteLine($"Check-in request JSON: {json}");
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var request = new HttpRequestMessage(HttpMethod.Post, BaseUrl + $"Booking/{bookingId}/checkin");
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _currentToken);
                request.Content = content;

                var response = await _httpClient.SendAsync(request);
                
                if (!response.IsSuccessStatusCode)
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine($"Check-in failed - Status: {response.StatusCode}, Response: {errorContent}");
                    
                    // Try to extract error message from response
                    string errorMessage = ExtractErrorMessage(errorContent);
                    if (string.IsNullOrEmpty(errorMessage))
                    {
                        errorMessage = $"Check-in failed with status {response.StatusCode}: {errorContent}";
                    }
                    
                    throw new Exception(errorMessage);
                }

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error checking in guest: {ex.Message}\n{ex.StackTrace}");
                throw; // Re-throw so caller can handle the error
            }
        }

        // Check-out guest using QR code
        public async Task<bool> CheckOutGuestAsync(int bookingId, string qrCodeData)
        {
            try
            {
                // Ensure we have a token
                if (string.IsNullOrEmpty(_currentToken))
                {
                    throw new Exception("You must be logged in to check out guests. Please log in first.");
                }

                // Note: Backend checkout endpoint doesn't require a body, just the bookingId in the route
                var request = new HttpRequestMessage(HttpMethod.Post, BaseUrl + $"Booking/{bookingId}/checkout");
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _currentToken);
                // No content needed - backend only checks booking status

                var response = await _httpClient.SendAsync(request);
                
                if (!response.IsSuccessStatusCode)
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine($"Check-out failed - Status: {response.StatusCode}, Response: {errorContent}");
                    
                    // Try to extract error message from response
                    string errorMessage = ExtractErrorMessage(errorContent);
                    if (string.IsNullOrEmpty(errorMessage))
                    {
                        errorMessage = $"Check-out failed with status {response.StatusCode}: {errorContent}";
                    }
                    
                    throw new Exception(errorMessage);
                }

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error checking out guest: {ex.Message}\n{ex.StackTrace}");
                throw; // Re-throw so caller can handle the error
            }
        }

        // Approve or reject a booking
        public async Task<bool> ApproveBookingAsync(int bookingId, bool isApproved, string rejectionReason = null)
        {
            try
            {
                // Ensure we have a token
                if (string.IsNullOrEmpty(_currentToken))
                {
                    throw new Exception("You must be logged in to approve bookings. Please log in first.");
                }

                var requestData = new
                {
                    isApproved = isApproved,
                    rejectionReason = rejectionReason
                };

                var json = SimpleJsonSerializer.Serialize(requestData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var request = new HttpRequestMessage(HttpMethod.Post, BaseUrl + $"Booking/{bookingId}/approve");
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _currentToken);
                request.Content = content;

                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error approving/rejecting booking: {ex.Message}");
                return false;
            }
        }

        private List<BookingResponse> DeserializeBookings(string json)
        {
            var bookings = new List<BookingResponse>();
            try
            {
                // Simple JSON parsing for bookings array
                int arrayStart = json.IndexOf('[');
                if (arrayStart < 0) return bookings;

                // Split by booking objects (simple approach)
                // For more complex scenarios, consider using a JSON library
                var bookingObjects = ExtractArrayObjects(json);

                foreach (var bookingJson in bookingObjects)
                {
                    var booking = DeserializeBooking(bookingJson);
                    if (booking != null && booking.Id > 0)
                        bookings.Add(booking);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error deserializing bookings: {ex.Message}");
            }
            return bookings;
        }

        private BookingResponse DeserializeBooking(string json)
        {
            try
            {
                var booking = new BookingResponse();

                SimpleJsonSerializer.ExtractNumericProperty(json, "id", out int id);
                booking.Id = id;

                SimpleJsonSerializer.ExtractStringProperty(json, "fullName", out string fullName);
                if (string.IsNullOrEmpty(fullName))
                    SimpleJsonSerializer.ExtractStringProperty(json, "FullName", out fullName);
                booking.FullName = fullName ?? string.Empty;

                SimpleJsonSerializer.ExtractStringProperty(json, "email", out string email);
                if (string.IsNullOrEmpty(email))
                    SimpleJsonSerializer.ExtractStringProperty(json, "Email", out email);
                booking.Email = email ?? string.Empty;

                SimpleJsonSerializer.ExtractStringProperty(json, "contact", out string contact);
                if (string.IsNullOrEmpty(contact))
                    SimpleJsonSerializer.ExtractStringProperty(json, "Contact", out contact);
                booking.Contact = contact ?? string.Empty;

                SimpleJsonSerializer.ExtractNumericProperty(json, "guestCount", out int guestCount);
                if (guestCount == 0)
                    SimpleJsonSerializer.ExtractNumericProperty(json, "GuestCount", out guestCount);
                booking.GuestCount = guestCount;

                SimpleJsonSerializer.ExtractStringProperty(json, "startDateTime", out string startDateTimeStr);
                if (string.IsNullOrEmpty(startDateTimeStr))
                    SimpleJsonSerializer.ExtractStringProperty(json, "StartDateTime", out startDateTimeStr);
                if (DateTime.TryParse(startDateTimeStr, out DateTime startDateTime))
                    booking.StartDateTime = startDateTime;

                SimpleJsonSerializer.ExtractStringProperty(json, "endDateTime", out string endDateTimeStr);
                if (string.IsNullOrEmpty(endDateTimeStr))
                    SimpleJsonSerializer.ExtractStringProperty(json, "EndDateTime", out endDateTimeStr);
                if (DateTime.TryParse(endDateTimeStr, out DateTime endDateTime))
                    booking.EndDateTime = endDateTime;

                SimpleJsonSerializer.ExtractStringProperty(json, "status", out string status);
                if (string.IsNullOrEmpty(status))
                    SimpleJsonSerializer.ExtractStringProperty(json, "Status", out status);
                booking.Status = status ?? "PendingApproval";

                SimpleJsonSerializer.ExtractStringProperty(json, "qrCodeData", out string qrCodeData);
                if (string.IsNullOrEmpty(qrCodeData))
                    SimpleJsonSerializer.ExtractStringProperty(json, "QrCodeData", out qrCodeData);
                booking.QrCodeData = qrCodeData;

                SimpleJsonSerializer.ExtractStringProperty(json, "notes", out string notes);
                if (string.IsNullOrEmpty(notes))
                    SimpleJsonSerializer.ExtractStringProperty(json, "Notes", out notes);
                booking.Notes = notes;

                SimpleJsonSerializer.ExtractStringProperty(json, "paymentImageUrl", out string paymentImageUrl);
                if (string.IsNullOrEmpty(paymentImageUrl))
                    SimpleJsonSerializer.ExtractStringProperty(json, "PaymentImageUrl", out paymentImageUrl);
                booking.PaymentImageUrl = paymentImageUrl;

                // Parse Condo summary
                int condoStart = json.IndexOf("\"condo\"", StringComparison.OrdinalIgnoreCase);
                if (condoStart >= 0)
                {
                    int condoObjStart = json.IndexOf('{', condoStart);
                    if (condoObjStart >= 0)
                    {
                        int condoObjEnd = FindMatchingBrace(json, condoObjStart);
                        if (condoObjEnd > condoObjStart)
                        {
                            string condoJson = json.Substring(condoObjStart, condoObjEnd - condoObjStart + 1);
                            var condo = new CondoSummary();

                            SimpleJsonSerializer.ExtractNumericProperty(condoJson, "id", out int condoId);
                            if (condoId == 0)
                                SimpleJsonSerializer.ExtractNumericProperty(condoJson, "Id", out condoId);
                            condo.Id = condoId;

                            SimpleJsonSerializer.ExtractStringProperty(condoJson, "name", out string condoName);
                            if (string.IsNullOrEmpty(condoName))
                                SimpleJsonSerializer.ExtractStringProperty(condoJson, "Name", out condoName);
                            condo.Name = condoName ?? string.Empty;

                            SimpleJsonSerializer.ExtractStringProperty(condoJson, "location", out string condoLocation);
                            if (string.IsNullOrEmpty(condoLocation))
                                SimpleJsonSerializer.ExtractStringProperty(condoJson, "Location", out condoLocation);
                            condo.Location = condoLocation ?? string.Empty;

                            booking.Condo = condo;
                        }
                    }
                }

                return booking;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error deserializing booking: {ex.Message}");
                return new BookingResponse(); // Return empty booking instead of null
            }
        }

        private List<string> ExtractArrayObjects(string json)
        {
            var objects = new List<string>();
            int depth = 0;
            int start = -1;

            for (int i = 0; i < json.Length; i++)
            {
                if (json[i] == '{')
                {
                    if (depth == 0) start = i;
                    depth++;
                }
                else if (json[i] == '}')
                {
                    depth--;
                    if (depth == 0 && start >= 0)
                    {
                        objects.Add(json.Substring(start, i - start + 1));
                        start = -1;
                    }
                }
            }

            return objects;
        }

        private int FindMatchingBrace(string json, int startIndex)
        {
            int depth = 0;
            for (int i = startIndex; i < json.Length; i++)
            {
                if (json[i] == '{') depth++;
                else if (json[i] == '}') depth--;
                if (depth == 0) return i;
            }
            return -1;
        }
    }

    // Simple JSON serializer for .NET Framework 4.7.2 (without external dependencies)
    // Note: This is a basic implementation. For production, consider using Newtonsoft.Json package
    public static class SimpleJsonSerializer
    {
        public static string Serialize(object obj)
        {
            var props = obj.GetType().GetProperties();
            var sb = new StringBuilder();
            sb.Append("{");

            for (int i = 0; i < props.Length; i++)
            {
                var prop = props[i];
                var value = prop.GetValue(obj);

                if (i > 0) sb.Append(",");

                sb.Append($"\"{prop.Name}\":");

                if (value == null)
                {
                    sb.Append("null");
                }
                else if (value is string str)
                {
                    // Properly escape all special JSON characters
                    string escaped = str
                        .Replace("\\", "\\\\")  // Escape backslashes first!
                        .Replace("\"", "\\\"")
                        .Replace("\n", "\\n")
                        .Replace("\r", "\\r")
                        .Replace("\t", "\\t")
                        .Replace("\f", "\\f")
                        .Replace("\b", "\\b");
                    sb.Append($"\"{escaped}\"");
                }
                else if (value is bool b)
                {
                    sb.Append(b ? "true" : "false");
                }
                else if (value is System.Collections.IEnumerable enumerable && !(value is string))
                {
                    sb.Append("[");
                    bool first = true;
                    foreach (var item in enumerable)
                    {
                        if (!first) sb.Append(",");
                        first = false;
                        if (item is string s)
                            sb.Append($"\"{s.Replace("\"", "\\\"")}\"");
                        else
                            sb.Append(item?.ToString() ?? "null");
                    }
                    sb.Append("]");
                }
                else
                {
                    sb.Append($"\"{value}\"");
                }
            }

            sb.Append("}");
            return sb.ToString();
        }

        public static LoginResponse Deserialize(string json)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"Deserializing JSON: {json}");
                var response = new LoginResponse();

                // Parse Token - look for "Token":"value"
                if (json.Contains("\"Token\"") || json.Contains("\"token\""))
                {
                    // Try "Token" first (capital T)
                    string searchPattern = "\"Token\":";
                    int tokenIndex = json.IndexOf(searchPattern, StringComparison.OrdinalIgnoreCase);

                    if (tokenIndex >= 0)
                    {
                        // Find the colon after Token
                        int colonIndex = tokenIndex + searchPattern.Length;

                        // Skip whitespace
                        while (colonIndex < json.Length && char.IsWhiteSpace(json[colonIndex]))
                            colonIndex++;

                        // Should be a quote after colon
                        if (colonIndex < json.Length && json[colonIndex] == '"')
                        {
                            colonIndex++; // Skip opening quote
                            int tokenEnd = colonIndex;

                            // Find closing quote (handle escaped quotes)
                            while (tokenEnd < json.Length)
                            {
                                if (json[tokenEnd] == '"' && (tokenEnd == colonIndex || json[tokenEnd - 1] != '\\'))
                                {
                                    break; // Found unescaped quote
                                }
                                tokenEnd++;
                            }

                            if (tokenEnd > colonIndex && tokenEnd < json.Length)
                            {
                                response.Token = json.Substring(colonIndex, tokenEnd - colonIndex);
                                System.Diagnostics.Debug.WriteLine($"Token extracted: {response.Token.Substring(0, Math.Min(20, response.Token.Length))}...");
                            }
                        }
                    }
                }

                // Parse User object (only if token was found, otherwise the response format might be wrong)
                if (!string.IsNullOrEmpty(response.Token))
                {
                    // Try both "User" and "user" (case-insensitive)
                    int userStart = json.IndexOf("\"User\":");
                    if (userStart < 0)
                    {
                        userStart = json.IndexOf("\"user\":");
                    }
                    
                    if (userStart >= 0)
                    {
                        System.Diagnostics.Debug.WriteLine($"Found user object at position: {userStart}");
                        userStart = json.IndexOf("{", userStart);
                        if (userStart >= 0)
                        {
                            int userEnd = FindMatchingBrace(json, userStart);
                            if (userEnd > userStart)
                            {
                                string userJson = json.Substring(userStart, userEnd - userStart + 1);
                                System.Diagnostics.Debug.WriteLine($"Extracted user JSON: {userJson}");
                                response.User = DeserializeUser(userJson);
                                System.Diagnostics.Debug.WriteLine($"User parsed - Username: {response.User.Username}, Email: {response.User.Email}, Roles count: {response.User.Roles.Count}");
                            }
                            else
                            {
                                System.Diagnostics.Debug.WriteLine("Could not find matching brace for user object");
                            }
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine("Could not find opening brace for user object");
                        }
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("User property not found in JSON (tried both 'User' and 'user')");
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("WARNING: Token was not found in JSON response. Response format might be incorrect.");
                }

                return response;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"JSON deserialization error: {ex.Message}");
                return null;
            }
        }

        private static UserInfo DeserializeUser(string json)
        {
            var user = new UserInfo();
            string originalJson = json; // Keep original for debugging
            json = json.Trim().TrimStart('{').TrimEnd('}');

            System.Diagnostics.Debug.WriteLine($"DeserializeUser - Input JSON: {originalJson}");

            // Extract properties
            ExtractStringProperty(json, "Id", out string id);
            ExtractStringProperty(json, "Username", out string username);
            ExtractStringProperty(json, "Email", out string email);

            user.Id = id;
            user.Username = username;
            user.Email = email;

            // Extract Roles array - improved logic
            System.Diagnostics.Debug.WriteLine($"Looking for Roles in JSON: {json}");
            
            if (json.Contains("\"Roles\"") || json.Contains("\"roles\""))
            {
                // Try case-sensitive first
                int rolesStart = json.IndexOf("\"Roles\":");
                if (rolesStart < 0)
                {
                    // Try lowercase
                    rolesStart = json.IndexOf("\"roles\":");
                }
                
                if (rolesStart >= 0)
                {
                    rolesStart += 8; // Skip "Roles":
                    System.Diagnostics.Debug.WriteLine($"Found Roles at position: {rolesStart}");
                    
                    // Skip whitespace
                    while (rolesStart < json.Length && char.IsWhiteSpace(json[rolesStart]))
                        rolesStart++;
                    
                    int arrayStart = json.IndexOf("[", rolesStart);
                    if (arrayStart >= 0)
                    {
                        System.Diagnostics.Debug.WriteLine($"Found array start at: {arrayStart}");
                        int arrayEnd = FindMatchingBrace(json, arrayStart, '[', ']');
                        if (arrayEnd > arrayStart)
                        {
                            string rolesJson = json.Substring(arrayStart + 1, arrayEnd - arrayStart - 1);
                            System.Diagnostics.Debug.WriteLine($"Roles JSON content: [{rolesJson}]");
                            
                            if (!string.IsNullOrWhiteSpace(rolesJson))
                            {
                                // Split by comma and clean each role
                                string[] roles = rolesJson.Split(',');
                                foreach (var role in roles)
                                {
                                    string cleanRole = role.Trim().Trim('"').Trim('[').Trim(']');
                                    if (!string.IsNullOrEmpty(cleanRole))
                                    {
                                        user.Roles.Add(cleanRole);
                                        System.Diagnostics.Debug.WriteLine($"Added role: {cleanRole}");
                                    }
                                }
                            }
                            else
                            {
                                System.Diagnostics.Debug.WriteLine("Roles array is empty");
                            }
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine($"Could not find array end. arrayStart: {arrayStart}");
                        }
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"Could not find array start after Roles: at position {rolesStart}");
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Roles property not found in JSON");
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("JSON does not contain 'Roles' property");
            }

            System.Diagnostics.Debug.WriteLine($"DeserializeUser result - Roles count: {user.Roles.Count}");
            return user;
        }

        public static bool ExtractStringProperty(string json, string propertyName, out string value)
        {
            value = string.Empty;
            string search = $"\"{propertyName}\":";
            int propStart = json.IndexOf(search);
            if (propStart >= 0)
            {
                int valueStart = propStart + search.Length;
                // Skip whitespace
                while (valueStart < json.Length && char.IsWhiteSpace(json[valueStart]))
                    valueStart++;

                // Check if value starts with quote (string value)
                if (valueStart >= json.Length || json[valueStart] != '"')
                    return false;

                valueStart++; // Skip opening quote
                int valueEnd = valueStart;

                // Find closing quote, properly handling escaped quotes
                while (valueEnd < json.Length)
                {
                    if (json[valueEnd] == '"')
                    {
                        // Check if this quote is escaped
                        int backslashes = 0;
                        int check = valueEnd - 1;
                        while (check >= valueStart && json[check] == '\\')
                        {
                            backslashes++;
                            check--;
                        }
                        // Odd number of backslashes means quote is escaped
                        if (backslashes % 2 == 0)
                        {
                            // This is the real closing quote
                            break;
                        }
                    }
                    valueEnd++;
                }

                if (valueEnd > valueStart && valueEnd < json.Length)
                {
                    string rawValue = json.Substring(valueStart, valueEnd - valueStart);
                    // Unescape JSON escape sequences (but process in reverse order to avoid double-replacement)
                    value = rawValue.Replace("\\\\", "\u0001")  // Temp marker for single backslash
                                    .Replace("\\\"", "\"")
                                    .Replace("\\n", "\n")
                                    .Replace("\\r", "\r")
                                    .Replace("\\t", "\t")
                                    .Replace("\u0001", "\\");  // Restore single backslashes
                    return true;
                }
            }
            return false;
        }

        private static int FindMatchingBrace(string json, int startIndex, char open = '{', char close = '}')
        {
            int depth = 0;
            for (int i = startIndex; i < json.Length; i++)
            {
                if (json[i] == open) depth++;
                else if (json[i] == close)
                {
                    depth--;
                    if (depth == 0) return i;
                }
            }
            return -1;
        }

        public static List<CondoResponse> DeserializeCondoList(string json)
        {
            List<CondoResponse> condos = new List<CondoResponse>();

            try
            {
                json = json.Trim();
                if (!json.StartsWith("[")) return condos;

                // Remove array brackets
                json = json.TrimStart('[').TrimEnd(']').Trim();

                // Split by object boundaries (simple approach - find { and matching })
                int i = 0;
                while (i < json.Length)
                {
                    // Find next object start
                    while (i < json.Length && json[i] != '{') i++;
                    if (i >= json.Length) break;

                    int objStart = i;
                    int objEnd = FindMatchingBrace(json, objStart, '{', '}');
                    if (objEnd <= objStart) break;

                    string objJson = json.Substring(objStart, objEnd - objStart + 1);
                    CondoResponse condo = DeserializeCondo(objJson);
                    if (condo != null) condos.Add(condo);

                    i = objEnd + 1;
                    // Skip comma if present
                    while (i < json.Length && (json[i] == ',' || char.IsWhiteSpace(json[i]))) i++;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error deserializing condo list: {ex.Message}");
            }

            return condos;
        }

        private static CondoResponse DeserializeCondo(string json)
        {
            try
            {
                var condo = new CondoResponse();
                json = json.Trim().TrimStart('{').TrimEnd('}');

                // Try both lowercase (from API) and PascalCase (standard)
                ExtractStringProperty(json, "name", out string name);
                if (string.IsNullOrEmpty(name))
                    ExtractStringProperty(json, "Name", out name);

                ExtractStringProperty(json, "location", out string location);
                if (string.IsNullOrEmpty(location))
                    ExtractStringProperty(json, "Location", out location);

                ExtractStringProperty(json, "description", out string description);
                if (string.IsNullOrEmpty(description))
                    ExtractStringProperty(json, "Description", out description);

                ExtractStringProperty(json, "amenities", out string amenities);
                if (string.IsNullOrEmpty(amenities))
                    ExtractStringProperty(json, "Amenities", out amenities);

                ExtractStringProperty(json, "imageUrl", out string imageUrl);
                if (string.IsNullOrEmpty(imageUrl))
                    ExtractStringProperty(json, "ImageUrl", out imageUrl);

                ExtractStringProperty(json, "status", out string status);
                if (string.IsNullOrEmpty(status))
                    ExtractStringProperty(json, "Status", out status);

                ExtractStringProperty(json, "bookingLink", out string bookingLink);
                if (string.IsNullOrEmpty(bookingLink))
                    ExtractStringProperty(json, "BookingLink", out bookingLink);

                // Extract numeric properties - try lowercase first, then PascalCase
                int id = 0;
                if (!ExtractNumericProperty(json, "id", out id))
                {
                    ExtractNumericProperty(json, "Id", out id);
                }
                if (id == 0)
                {
                    ExtractNumericProperty(json, "ID", out id);
                }

                int maxGuests = 0;
                if (!ExtractNumericProperty(json, "maxGuests", out maxGuests))
                {
                    ExtractNumericProperty(json, "MaxGuests", out maxGuests);
                }

                decimal pricePerNight = 0;
                if (!ExtractDecimalProperty(json, "pricePerNight", out pricePerNight))
                {
                    ExtractDecimalProperty(json, "PricePerNight", out pricePerNight);
                }

                condo.Id = id;
                condo.Name = name ?? string.Empty;
                condo.Location = location ?? string.Empty;
                condo.Description = description ?? string.Empty;
                condo.Amenities = amenities ?? string.Empty;
                condo.MaxGuests = maxGuests;
                condo.PricePerNight = pricePerNight;
                condo.ImageUrl = imageUrl ?? string.Empty;
                condo.Status = status ?? string.Empty;
                condo.BookingLink = bookingLink ?? string.Empty;

                System.Diagnostics.Debug.WriteLine($"DeserializeCondo: Parsed condo '{condo.Name}' with Id = {condo.Id}");

                return condo;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error deserializing condo: {ex.Message}");
                return null;
            }
        }

        public static bool ExtractNumericProperty(string json, string propertyName, out int value)
        {
            value = 0;
            string search = $"\"{propertyName}\":";
            int propStart = json.IndexOf(search, StringComparison.OrdinalIgnoreCase);
            if (propStart >= 0)
            {
                int valueStart = propStart + search.Length;
                // Skip whitespace
                while (valueStart < json.Length && char.IsWhiteSpace(json[valueStart]))
                    valueStart++;

                // Find the end of the numeric value (comma, }, or whitespace)
                int valueEnd = valueStart;
                while (valueEnd < json.Length &&
                       json[valueEnd] != ',' &&
                       json[valueEnd] != '}' &&
                       json[valueEnd] != ']' &&
                       !char.IsWhiteSpace(json[valueEnd]))
                {
                    valueEnd++;
                }

                if (valueEnd > valueStart)
                {
                    string numStr = json.Substring(valueStart, valueEnd - valueStart).Trim();
                    if (int.TryParse(numStr, out value))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool ExtractDecimalProperty(string json, string propertyName, out decimal value)
        {
            value = 0;
            string search = $"\"{propertyName}\":";
            int propStart = json.IndexOf(search, StringComparison.OrdinalIgnoreCase);
            if (propStart >= 0)
            {
                int valueStart = propStart + search.Length;
                // Skip whitespace
                while (valueStart < json.Length && char.IsWhiteSpace(json[valueStart]))
                    valueStart++;

                // Find the end of the numeric value (comma, }, or whitespace)
                int valueEnd = valueStart;
                while (valueEnd < json.Length &&
                       json[valueEnd] != ',' &&
                       json[valueEnd] != '}' &&
                       json[valueEnd] != ']' &&
                       !char.IsWhiteSpace(json[valueEnd]))
                {
                    valueEnd++;
                }

                if (valueEnd > valueStart)
                {
                    string numStr = json.Substring(valueStart, valueEnd - valueStart).Trim();
                    if (decimal.TryParse(numStr, out value))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}

