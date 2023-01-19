namespace MochiApi.Dtos
{
    public abstract class BaseEntityDto
    {
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
