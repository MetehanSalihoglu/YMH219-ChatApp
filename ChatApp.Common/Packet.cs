namespace ChatApp.Common;

/// <summary>Istemci ile sunucu arasindaki mesaj turleri.</summary>
public enum PacketType
{
    /// <summary>Istemci -> Sunucu: kullanici adi ile katilma istegi.</summary>
    Join,

    /// <summary>Herkese acik sohbet mesaji.</summary>
    Message,

    /// <summary>Ozel mesaj.</summary>
    Private,

    /// <summary>Sunucu -> Istemci: sistem bildirimi (katildi, ayrildi, hata).</summary>
    System,

    /// <summary>Sunucu -> Istemci: cevrimici kullanici listesi.</summary>
    Users,

    /// <summary>Katilma reddi (ornegin kullanici adi kullanimda).</summary>
    Reject
}

/// <summary>
/// Tel uzerinden gonderilen tek bir satirlik paket.
/// Bicim: <c>TUR|alan1|alan2|...</c>; metin alanlarindaki ozel karakterler kacislanir.
/// </summary>
public sealed record Packet(PacketType Type, params string[] Fields)
{
    private const char Separator = '|';

    /// <summary>Paketi tek satirlik metne cevirir.</summary>
    public string Serialize()
    {
        return string.Join(Separator, new[] { Type.ToString() }.Concat(Fields.Select(Escape)));
    }

    /// <summary>Satiri paket nesnesine cevirir; bicim hataliysa false doner.</summary>
    public static bool TryParse(string? line, out Packet? packet)
    {
        packet = null;
        if (string.IsNullOrWhiteSpace(line))
        {
            return false;
        }

        string[] parts = line.Split(Separator);
        if (!Enum.TryParse(parts[0], out PacketType type) || !Enum.IsDefined(type))
        {
            return false;
        }

        packet = new Packet(type, parts.Skip(1).Select(Unescape).ToArray());
        return true;
    }

    /// <summary>Verilen indeksteki alan; yoksa bos metin.</summary>
    public string Field(int index) => index < Fields.Length ? Fields[index] : string.Empty;

    private static string Escape(string text) =>
        text.Replace("\\", "\\\\").Replace("|", "\\p").Replace("\r", "").Replace("\n", "\\n");

    private static string Unescape(string text)
    {
        var sb = new System.Text.StringBuilder();
        for (int i = 0; i < text.Length; i++)
        {
            if (text[i] == '\\' && i + 1 < text.Length)
            {
                i++;
                sb.Append(text[i] switch { 'p' => '|', 'n' => '\n', _ => text[i] });
            }
            else
            {
                sb.Append(text[i]);
            }
        }
        return sb.ToString();
    }
}
