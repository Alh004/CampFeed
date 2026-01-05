using NUnit.Framework;
using KlasseLib;
f
namespace TestProject1;

public class Tests
{
    [Test]
    public void GetQrUrl_HasToken_ReturnsRoomTokenUrl()
    {
        var room = new Room(1, "A101", "B1", "abc123");

        var url = room.GetQrUrl("https://example.com/issues");

        Assert.AreEqual("https://example.com/issues?roomToken=abc123", url);
    }

    [Test]
    public void GetQrUrl_NoToken_ReturnsRoomIdUrl()
    {
        var room = new Room(7, "C202", "B2", null);

        var url = room.GetQrUrl("https://example.com/issues");

        Assert.AreEqual("https://example.com/issues?roomId=7", url);
    }

    [Test]
    public void GetQrUrl_EmptyToken_ReturnsRoomIdUrl()
    {
        var room = new Room(9, "D303", "B3", "");

        var url = room.GetQrUrl("https://example.com/issues");

        Assert.AreEqual("https://example.com/issues?roomId=9", url);
    }

    [Test]
    public void Constructor_SetsAllProperties()
    {
        var room = new Room(5, "E404", "B4", "tok123");

        Assert.AreEqual(5, room.RoomId);
        Assert.AreEqual("E404", room.RoomName);
        Assert.AreEqual("B4", room.Building);
        Assert.AreEqual("tok123", room.QrCodeToken);
    }

    [Test]
    public void ParameterlessConstructor_CanSetProperties()
    {
        var room = new Room
        {
            RoomId = 2,
            RoomName = "F505",
            Building = "B5",
            QrCodeToken = "zzz"
        };

        Assert.AreEqual(2, room.RoomId);
        Assert.AreEqual("F505", room.RoomName);
        Assert.AreEqual("B5", room.Building);
        Assert.AreEqual("zzz", room.QrCodeToken);
    }

    [Test]
    public void ToString_ReturnsExpectedFormat_WithToken()
    {
        var room = new Room(3, "G606", "B6", "abc123");

        var s = room.ToString();

        Assert.AreEqual("RoomId=3, RoomName=G606, Building=B6, QrCodeToken=abc123", s);
    }

    [Test]
    public void ToString_ReturnsExpectedFormat_WithoutToken()
    {
        var room = new Room(3, "G606", "B6", null);

        var s = room.ToString();

        Assert.AreEqual("RoomId=3, RoomName=G606, Building=B6, QrCodeToken=", s);
    }
}
