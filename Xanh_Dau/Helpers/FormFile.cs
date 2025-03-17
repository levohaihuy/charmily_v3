namespace Xanh_Dau.Helpers;

public class FormFile : IFormFile
{
    private readonly Stream _stream;
    private readonly long _length;
    private readonly string _fileName;

    public FormFile(Stream stream, long length, string name, string fileName)
    {
        _stream = stream;
        _length = length;
        Name = name;
        _fileName = fileName;
    }

    public string ContentType => "image/png";
    public string ContentDisposition => string.Empty;
    public IHeaderDictionary Headers => new HeaderDictionary();
    public long Length => _length;
    public string Name { get; }
    public string FileName => _fileName;

    public void CopyTo(Stream target)
    {
        _stream.CopyTo(target);
    }

    public async Task CopyToAsync(Stream target, CancellationToken cancellationToken = default)
    {
        await _stream.CopyToAsync(target, cancellationToken);
    }

    public Stream OpenReadStream()
    {
        _stream.Position = 0;
        return _stream;
    }
}