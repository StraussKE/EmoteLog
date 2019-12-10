using System;
using System.Collections.Generic;
using Xunit;
using EmoteLog.Models;
using EmoteLog.Repositories;
using EmoteLog.Controllers;
using System.Linq;

namespace EmoteLog.Tests
{
    public class MemberControllerTests
    {
        [Fact]
        public void TestNewEntry()
        {
            // Arrange
            var testLogRepo = new FakeLogRepository();
            var testMemberController = new MemberController(testLogRepo);

            var testLogEntry1 = new LogEntry
            {
                LogId = new Guid(), // in production, this is set in the database
                UserId = "testId1",
                // PublishDate = DateTime.Now,  This will be set by the controller
                Mood = 5,
                Entry = "Test entry"
            };

            // Act
            testMemberController.NewEntry(testLogEntry1);

            // Assert
            Assert.NotEmpty(testLogRepo.Entries);
            Assert.Equal("Test entry", testLogRepo.Entries.Last().Entry);
            Assert.IsType<DateTime>(testLogRepo.Entries.Last().PublishDate);
            Assert.Equal(1, testLogRepo.Entries.Count());
        }

        [Fact]
        public void TestAverageMood()
        {
            // Arrange
            var testLogRepo = new FakeLogRepository(); // not going to be used in this test
            var testMemberController = new MemberController(testLogRepo);
            var testLogList = new List<LogEntry>(){
                new LogEntry
                {
                    LogId = new Guid(),
                    UserId = "testId1",
                    PublishDate = DateTime.Now,
                    Mood = 4,
                    Entry = "I'm feeling fairly flat today.  It's not a good day, it's not a bad day, it's just a day."
                },
                new LogEntry
                {
                    LogId = new Guid(),
                    UserId = "testId1",
                    PublishDate = DateTime.Now,
                    Mood = 6,
                    Entry = "Today is the best day ever!"
                }
            };

            // Act
            int avgResult = testMemberController.AverageMood(testLogList);

            // Assert
            Assert.Equal(5, avgResult);
        }
    }
}
