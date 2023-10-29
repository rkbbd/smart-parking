namespace SPSApps.Models
{
    public class BaseModel<T>
    {
        public T Id { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public long CreatedBy { get; set; }
    }
}
