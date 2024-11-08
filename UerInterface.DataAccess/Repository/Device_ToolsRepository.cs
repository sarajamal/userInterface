﻿using Test12.DataAccess.Data;
using Test12.DataAccess.Repository.IRepository;
using Test12.Models.Models.Device_Tools;

namespace Test12.DataAccess.Repository
{
    public class Device_ToolsRepository : Repository<DevicesAndTools>, IDevice_ToolsRepository
    {
        private readonly ApplicationDbContext _context;
        public Device_ToolsRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(DevicesAndTools obj)
        {
            var objFormDb = _context.DevicesAndTools.FirstOrDefault(u => u.DevicesAndToolsID == obj.DevicesAndToolsID);
            if (objFormDb != null)
            {
                objFormDb.DevicesAndTools_Name = obj.DevicesAndTools_Name;
                objFormDb.DevicesAndToolsOrder = obj.DevicesAndToolsOrder;
                objFormDb.DevicesAndTools_Num = obj.DevicesAndTools_Num;
                if (obj.DevicesAndTools_Image != null)
                {
                    objFormDb.DevicesAndTools_Image = obj.DevicesAndTools_Image;
                }
                _context.SaveChanges();
            }
        }
        // New function to get the last ID
        public int GetLastStepId()
        {
            // If there are no entries in the table, return 0 or an appropriate default value
            if (!_context.DevicesAndTools.Any())
            {
                return 0;
            }
            // Retrieve and return the max PrepStepsID
            return _context.DevicesAndTools.Max(p => p.DevicesAndToolsID);
        }
    }
}
