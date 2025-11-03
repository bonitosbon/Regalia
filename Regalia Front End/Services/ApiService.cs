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
                    sb.Append($"\"{str.Replace("\"", "\\\"").Replace("\n", "\\n").Replace("\r", "\\r")}\"");
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
                    int userStart = json.IndexOf("\"User\":");
                    if (userStart >= 0)
                    {
                        userStart = json.IndexOf("{", userStart);
                        if (userStart >= 0)
                        {
                            int userEnd = FindMatchingBrace(json, userStart);
                            if (userEnd > userStart)
                            {
                                string userJson = json.Substring(userStart, userEnd - userStart + 1);
                                response.User = DeserializeUser(userJson);
                                System.Diagnostics.Debug.WriteLine($"User parsed - Username: {response.User.Username}, Email: {response.User.Email}");
                            }
                        }
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
            json = json.Trim().TrimStart('{').TrimEnd('}');

            // Extract properties
            ExtractStringProperty(json, "Id", out string id);
            ExtractStringProperty(json, "Username", out string username);
            ExtractStringProperty(json, "Email", out string email);
            
            user.Id = id;
            user.Username = username;
            user.Email = email;

            // Extract Roles array
            if (json.Contains("\"Roles\""))
            {
                int rolesStart = json.IndexOf("\"Roles\":") + 8;
                int arrayStart = json.IndexOf("[", rolesStart);
                if (arrayStart >= 0)
                {
                    int arrayEnd = FindMatchingBrace(json, arrayStart, '[', ']');
                    if (arrayEnd > arrayStart)
                    {
                        string rolesJson = json.Substring(arrayStart + 1, arrayEnd - arrayStart - 1);
                        string[] roles = rolesJson.Split(',');
                        foreach (var role in roles)
                        {
                            string cleanRole = role.Trim().Trim('"');
                            if (!string.IsNullOrEmpty(cleanRole))
                                user.Roles.Add(cleanRole);
                        }
                    }
                }
            }

            return user;
        }

        private static void ExtractStringProperty(string json, string propertyName, out string value)
        {
            value = string.Empty;
            string search = $"\"{propertyName}\":";
            int start = json.IndexOf(search);
            if (start >= 0)
            {
                start += search.Length;
                int end = json.IndexOf("\"", start + 1);
                if (end > start)
                {
                    value = json.Substring(start + 1, end - start - 1);
                }
            }
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
    }
}

