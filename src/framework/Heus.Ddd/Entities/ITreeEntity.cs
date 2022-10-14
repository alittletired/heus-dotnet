namespace Heus.Ddd.Entities;

public interface ITreeEntity
{
    public int Sort { get; set; }
    public  string TreeCode { get; set; }
    public  string TreePath { get; set; }
    public long? ParentId { get; set; }

}