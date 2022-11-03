namespace Heus.Ddd.Entities;

public interface ITreeEntity
{
    public int Sort { get; set; }
    public  string Code { get; set; }
    public  string TreeCode { get; set; }
    public long? ParentId { get; set; }

}