namespace RTN.API.Shared;

public class EncryptionSettings {
    public required string Key { get; set; }
    public required string IV { get; set; }
}

public class AppSettings {
    public required EncryptionSettings EncryptionSettings { get; set; }
}
