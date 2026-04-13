using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace BimManagerPortal.WebAssembly.Auth;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly IJSRuntime _js;

    public CustomAuthStateProvider(IJSRuntime js) => _js = js;

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await _js.InvokeAsync<string?>("localStorage.getItem", "authToken");

        if (string.IsNullOrEmpty(token) || IsTokenExpired(token))
            return Unauthenticated();

        var claims = ParseClaimsFromJwt(token);
        var identity = new ClaimsIdentity(claims, "jwt", "name", "role");
        return new AuthenticationState(new ClaimsPrincipal(identity));
    }

    public async Task NotifyUserAuthenticatedAsync(string token)
    {
        await _js.InvokeVoidAsync("localStorage.setItem", "authToken", token);
        var claims = ParseClaimsFromJwt(token);
        var identity = new ClaimsIdentity(claims, "jwt", "name", "role");
        NotifyAuthenticationStateChanged(
            Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity))));
    }

    public async Task NotifyUserLoggedOutAsync()
    {
        await _js.InvokeVoidAsync("localStorage.removeItem", "authToken");
        NotifyAuthenticationStateChanged(Task.FromResult(Unauthenticated()));
    }

    public async Task<string?> GetTokenAsync()
    {
        var token = await _js.InvokeAsync<string?>("localStorage.getItem", "authToken");
        return string.IsNullOrEmpty(token) || IsTokenExpired(token) ? null : token;
    }

    private static AuthenticationState Unauthenticated() =>
        new(new ClaimsPrincipal(new ClaimsIdentity()));

    private static bool IsTokenExpired(string jwt)
    {
        try
        {
            var payload = jwt.Split('.')[1];
            var json = System.Text.Encoding.UTF8.GetString(ParseBase64(payload));
            var doc = JsonDocument.Parse(json);
            if (doc.RootElement.TryGetProperty("exp", out var exp))
                return DateTimeOffset.UtcNow.ToUnixTimeSeconds() >= exp.GetInt64();
            return false;
        }
        catch
        {
            return true;
        }
    }

    private static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var payload = jwt.Split('.')[1];
        var json = System.Text.Encoding.UTF8.GetString(ParseBase64(payload));
        var kvp = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(json)!;
        return kvp.Select(p => new Claim(p.Key, p.Value.ToString()));
    }

    private static byte[] ParseBase64(string base64)
    {
        base64 = base64.Replace('-', '+').Replace('_', '/');
        base64 = base64.PadRight(base64.Length + (4 - base64.Length % 4) % 4, '=');
        return Convert.FromBase64String(base64);
    }
}
