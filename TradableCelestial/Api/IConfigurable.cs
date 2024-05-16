namespace TradableCelestial.Api;

public interface IConfigurable
{
    /// <summary>
    /// The unique ID of this configuration
    /// </summary>
    public string Id { get; }

    /// <summary>
    /// The name entry in config menu
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// The explanatory info for this entry
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// Whether this config is mandatory
    /// </summary>
    public bool Mandatory { get; }
}
