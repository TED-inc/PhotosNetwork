using SQLite4Unity3d;

public abstract class LocalServiceBase
{
    protected readonly SQLiteConnection connection;

    public LocalServiceBase(SQLiteConnection connection) =>
        this.connection = connection;
}
