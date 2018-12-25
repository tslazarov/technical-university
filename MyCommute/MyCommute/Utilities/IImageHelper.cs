﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCommute.Utilities
{
    public interface IImageHelper
    {
        string UploadImage(IFormFile image, string imageName);
    }
}