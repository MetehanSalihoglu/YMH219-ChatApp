namespace ChatApp.Common;

/// <summary>Sunucu ve istemci tarafinda ortak kullanilan sabitler.</summary>
public static class ChatDefaults
{
    public const int Port = 5000;

    public const string Host = "127.0.0.1";

    public const int MaxUserNameLength = 20;

    public const int MaxMessageLength = 500;

    /// <summary>Kullanici adini dogrular (bos olamaz, '|' ve satir sonu icermez).</summary>
    public static bool IsValidUserName(string? name, out string error)
    {
        name = name?.Trim();
        if (string.IsNullOrEmpty(name))
        {
            error = "Kullanıcı adı boş olamaz.";
            return false;
        }
        if (name.Length > MaxUserNameLength)
        {
            error = $"Kullanıcı adı en fazla {MaxUserNameLength} karakter olabilir.";
            return false;
        }
        if (name.Any(c => char.IsControl(c) || c is '|' or ','))
        {
            error = "Kullanıcı adı '|' veya ',' içeremez.";
            return false;
        }
        error = string.Empty;
        return true;
    }
}
