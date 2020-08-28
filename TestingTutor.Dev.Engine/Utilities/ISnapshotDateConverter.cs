using System;

namespace TestingTutor.Dev.Engine.Utilities
{
    public interface ISnapshotDateConverter
    {
        bool CanConvert(string name);
        DateTime Convert(string name);
    }
}
