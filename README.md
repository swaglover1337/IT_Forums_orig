# IT_Forums_COPY1

private static ITForum2BDEntities1 _instance;

public ITForum2BDEntities1()
    : base("name=ITForum2BDEntities1")
{
}

public static ITForum2BDEntities1 GetInst()
{
    if (_instance == null)
        _instance = new ITForum2BDEntities1();
    return _instance;
}
