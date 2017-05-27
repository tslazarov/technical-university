﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmarterThanYou.Server.Models
{
    public class QuestionViewModel
    {
        public string Category { get; set; }

        public string Question { get; set; }

        public string Answer { get; set; }

        public List<string> Answers { get; set; }
    }
}