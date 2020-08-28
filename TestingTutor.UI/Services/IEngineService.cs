using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestingTutor.Dev.Data.Dtos;
using TestingTutor.Dev.Data.Models;
using TestingTutor.Dev.Engine.Options;

namespace TestingTutor.UI.Services
{
    public interface IEngineService
    {
        void RunPreAssignment(int id);
        void RunSubmission(StudentSubmissionDto submission);
        void RunMarkovModel(int id, MarkovModelOptions options);
    }
}
