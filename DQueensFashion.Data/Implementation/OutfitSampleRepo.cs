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
    public class OutfitSampleRepo : CoreRepo<OutfitSample>, IOutfitSampleRepo
    {
        public OutfitSampleRepo(DbContext ctx) : base(ctx)
        {

        }
    }
}