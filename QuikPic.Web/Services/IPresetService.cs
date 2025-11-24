using System;
using QuikPic.Web.Models;

namespace QuikPic.Web.Services;

public interface IPresetService
{
    int Add(Preset preset);
    Preset GetById(int id);
    IEnumerable<Preset> GetAllPresets();
    void EditPreset(Preset preset);
    bool DeleteById(int id);
}
