using System;
using System.Collections.Generic;
using System.Text;

namespace HolidayTracker.Modules
{
    public enum DataResponse
    {
        Success,
        DuplicateCreated,
        DatabaseFailure,
        SplitHolidayCreated
    }

    public delegate DataResponse DataAccessEventHandler(object sender, EventArgs args);
}
