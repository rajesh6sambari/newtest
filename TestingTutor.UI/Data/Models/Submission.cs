using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using TestingTutor.Models;

namespace TestingTutor.UI.Data.Models
{
    public class Submission
    {
        public int Id { get; set; }
        [DisplayName("Submitter ID")]
        public string SubmitterId { get; set; }
        [DisplayName("Assignment")]
        public int AssignmentId { get; set; }
        [DisplayName("Assignment")]
        public Assignment Assignment { get; set; }
        [Display(Name="Application Mode")]
        public string ApplicationMode { get; set; }
        [DisplayName("Feedback")]
        public int? FeedbackId { get; set; }
        public Feedback Feedback { get; set; }
        [DisplayName("Submitted"), DisplayFormat(DataFormatString = "{0:MM/dd/yy hh:mm:ss tt}")]
        public DateTime SubmissionDateTime { get; set; }
        public byte[] SubmitterSolution { get; set; }
        public byte[] SubmitterTestCaseSolution { get; set; }
        public string Notes { get; set; }
    }
}
