namespace FreeCourse.Services.Catalog.Settings
{
    public interface IDatabaseSetting
    {
        public string CourseCollectionName { get; set; }
        public string CategoryCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string  DatabaseName { get; set; }
    }
}
