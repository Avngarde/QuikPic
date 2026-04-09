using System;
using QuikPic.Web.Models;

namespace QuikPic.Web.Services;

public class PresetService : IPresetService
{
    private readonly QuikPicContext _qpContext;

    public PresetService(QuikPicContext qpContext)
    {
        _qpContext = qpContext;
    }

    public int Add(Preset preset)
    {
        _qpContext.Add(preset);
        return _qpContext.SaveChanges();
    }

    public bool DeleteById(int id)
    {
        var preset = _qpContext.Presets.Find(id);
        if (preset == null) return false;

        _qpContext.Presets.Remove(preset);
        return _qpContext.SaveChanges() > 0;
    }

    public void EditPreset(Preset preset)
    {
        _qpContext.Presets.Update(preset);
        _qpContext.SaveChanges();
    }

    public IEnumerable<Preset> GetAllPresets()
    {
        return _qpContext.Presets.ToList();
    }

    public Preset GetById(int id)
    {
        Preset? preset = _qpContext.Presets.Find(id);
        if (preset is not null)
            return preset;
        else
            return new Preset();
    }
}
