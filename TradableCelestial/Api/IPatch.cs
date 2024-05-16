namespace TradableCelestial.Api;

public interface IPatch : IConfigurable
{
    /// <summary>
    /// The patch itself
    /// </summary>
    public void Commit();
}
