using System;
using System.Linq;
using Xunit;
using EmoteLog.Models;
using EmoteLog.Repositories;

namespace EmoteLog.Tests
{
    public class LogRepoTests
    {
        [Fact]
        public void TestDirectLogCreation()
        {
            // Arrange
            var testLogId = new Guid();
            var testUserId = "testId1";
            var testPublishDate = DateTime.Now;
            var testMood = 5;
            var testEntry = "I'm feeling fairly flat today.  It's not a good day, it's not a bad day, it's just a day.";

            // Act
            var testLogEntry = new LogEntry
            {
                LogId = testLogId,
                UserId = testUserId,
                PublishDate = testPublishDate,
                Mood = testMood,
                Entry = testEntry
            };

            // Assert
            Assert.NotNull(testLogEntry);
            Assert.Equal("testId1", testLogEntry.UserId);
            Assert.Equal(5, testLogEntry.Mood);
        }

        [Fact]
        public void TestDirectLogRepoAddEntry()
        {
            // Arrange
            var testLogEntry1 = new LogEntry
            {
                LogId = new Guid(),
                UserId = "testId1",
                PublishDate = DateTime.Now,
                Mood = 5,
                Entry = "I'm feeling fairly flat today.  It's not a good day, it's not a bad day, it's just a day."
            };
            var testLogEntry2 = new LogEntry
            {
                LogId = new Guid(),
                UserId = "testId1",
                PublishDate = DateTime.Now,
                Mood = 10,
                Entry = "Today is the best day ever!"
            };

            var testLogRepo = new FakeLogRepository();

            // Act
            testLogRepo.AddEntry(testLogEntry1);
            testLogRepo.AddEntry(testLogEntry2);

            // Assert
            Assert.Equal(2, testLogRepo.Entries.Count());
            Assert.Equal(10, testLogRepo.Entries.ToList().Last().Mood);
        }

        [Fact]
        public void TestQueryable()
        {
            // Arrange
            var testLogRepo = new FakeLogRepository();

            testLogRepo.AddEntry(
                new LogEntry
                {
                    LogId = new Guid(),
                    UserId = "testId1",
                    PublishDate = DateTime.Now,
                    Mood = 5,
                    Entry = "I'm feeling fairly flat today.  It's not a good day, it's not a bad day, it's just a day."
                });
            testLogRepo.AddEntry(
                new LogEntry
                {
                    LogId = new Guid(),
                    UserId = "testId1",
                    PublishDate = DateTime.Now,
                    Mood = 10,
                    Entry = "Today is the best day ever!"
                });


            // Act
            var countTestId1 = testLogRepo.Entries.Where(e => e.UserId == "testId1").ToList().Count();
            var countMood10 = testLogRepo.Entries.Where(e => e.Mood == 10).ToList().Count();

            // Assert
            Assert.Equal(2, countTestId1);
            Assert.Equal(1, countMood10);
        }
    }
}
