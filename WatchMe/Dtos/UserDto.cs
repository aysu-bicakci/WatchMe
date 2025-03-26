public class UserDto
{
    public string? Nickname { get; set; }  // Giriş için opsiyonel
    public required string Email { get; set; }  // Kayıt ve girişte zorunlu
    public required string Password { get; set; }  // Kayıt ve girişte zorunlu
}