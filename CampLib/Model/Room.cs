namespace KlasseLib;
d
public class Room
{
    public int RoomId { get; set; }
    public string RoomName { get; set; } = string.Empty;
    public string Building { get; set; } = string.Empty;

    // Bruges til QR-flow: fx "abc123", som du kan sætte ind i en URL
    public string? QrCodeToken { get; set; }

    public Room()
    {
    }

    public Room(int roomId, string roomName, string building, string? qrCodeToken = null)
    {
        RoomId = roomId;
        RoomName = roomName;
        Building = building;
        QrCodeToken = qrCodeToken;
    }

    // Hjælpe-metode: byg en URL til fejlmeldingssiden for det her lokale
    public string GetQrUrl(string baseUrl)
    {
        if (!string.IsNullOrEmpty(QrCodeToken))
        {
            return $"{baseUrl}?roomToken={QrCodeToken}";
        }

        // fallback: brug bare RoomId, hvis der ikke er sat token
        return $"{baseUrl}?roomId={RoomId}";
    }

    public override string ToString()
    {
        return $"RoomId={RoomId}, RoomName={RoomName}, Building={Building}, QrCodeToken={QrCodeToken}";
    }
}