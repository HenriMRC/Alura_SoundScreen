namespace screensound.database.dal;

internal class DAL<T> : BaseDAL<T, ScreenSoundContext> where T : class
{
    public DAL(ScreenSoundContext context) : base(context) { }
}
