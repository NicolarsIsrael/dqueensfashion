﻿using DQueensFashion.Core.Model;
using DQueensFashion.Data.Contract;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DQueensFashion.Data.Implementation
{
    public class CategoryRepo : CoreRepo<Category>, ICategoryRepo
    {
        public CategoryRepo(DbContext ctx) : base(ctx)
        {

        }

        //other functions
    }

}
