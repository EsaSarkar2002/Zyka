using Microsoft.AspNetCore.Identity;
using Zyka.Models.Enums;
using Zyka.Models;
using Zyka.Security;

namespace Zyka.Data
{
    public class DbSeeder
    {
        public static void Seed(ZykaDbContext context)
        {
            // Implement seeding logic here
            if (!context.Users.Any())
            {
                var adminUser = new User
                {
                    UserName = "Admin",
                    EmailAddress = "admin@zyka.com",
                    Role = UserRole.Admin,
                    IsActive = true,
                    HashedPassword = PasswordHasher.Hash("Admin@123")
                };
                context.Users.Add(adminUser);
                context.SaveChanges();
            }

            if (!context.TimeSlots.Any())
            {
                var timeSlots = new List<TimeSlot>();

                TimeSpan openingTime = new TimeSpan(9, 0, 0);
                TimeSpan closingTime = new TimeSpan(21, 0, 0);
                TimeSpan slotDuration=new TimeSpan(2, 0, 0);
                TimeSpan slotStep=new TimeSpan(1, 0, 0);
                for (TimeSpan start = openingTime; start + slotDuration <= closingTime; start += slotStep)
                {
                    TimeSpan end = start + slotDuration;
                    var period = start.Hours < 12 ? TimeSlotPeriod.Morning : (start.Hours < 17 ? TimeSlotPeriod.Afternoon : TimeSlotPeriod.Evening);
                    var displayText = $"{start:hh\\:mm} - {end:hh\\:mm}";
                    timeSlots.Add(new TimeSlot
                    {
                        StartTime = start,
                        EndTime = end,
                        Period = period,
                        DisplayText = displayText,
                        IsActive = true
                    });
                }

                context.TimeSlots.AddRange(timeSlots);
                context.SaveChanges();
            }

            if (!context.Tables.Any())
            {
                var tables = new List<TableInfo>
    {
        new TableInfo
        {
            TableNumber = "T1",
            Category = TableCategory.Family,
            Status = TableStatus.Available,
            IsActive = true
        },
        new TableInfo
        {
            TableNumber = "T2",
            Category = TableCategory.Family,
            Status = TableStatus.Available,
            IsActive = true
        },
        new TableInfo
        {
            TableNumber = "T3",
            Category = TableCategory.Family,
            Status = TableStatus.Available,
            IsActive = true
        },
        new TableInfo
        {
            TableNumber = "T4",
            Category = TableCategory.Date,
            Status = TableStatus.Available,
            IsActive = true
        },
        new TableInfo
        {
            TableNumber = "T5",
            Category = TableCategory.Date,
            Status = TableStatus.Available,
            IsActive = true
        },
        new TableInfo
        {
            TableNumber = "T6",
            Category = TableCategory.Date,
            Status = TableStatus.Available,
            IsActive = true
        },
        new TableInfo
        {
            TableNumber = "T7",
            Category = TableCategory.Date,
            Status = TableStatus.Available,
            IsActive = true
        },
        new TableInfo
        {
            TableNumber = "T8",
            Category = TableCategory.Celebration,
            Status = TableStatus.Available,
            IsActive = true
        },
        new TableInfo
        {
            TableNumber = "T9",
            Category = TableCategory.Meeting,
            Status = TableStatus.Available,
            IsActive = true
        },
        new TableInfo
        {
            TableNumber = "T10",
            Category = TableCategory.Meeting,
            Status = TableStatus.Available,
            IsActive = true
        }
    };

                context.Tables.AddRange(tables);
                context.SaveChanges();
            }
        }
    }
}
