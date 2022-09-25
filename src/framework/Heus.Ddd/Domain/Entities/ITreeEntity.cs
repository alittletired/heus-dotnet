namespace Heus.Ddd.Data;

public interface ITreeEntity
{
    public int Sort { get; set; }
    public  string TreeCode { get; set; }
    public  string TreePath { get; set; }
    public EntityId? ParentId { get; set; }

}