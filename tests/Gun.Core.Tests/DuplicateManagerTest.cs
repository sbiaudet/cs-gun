using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Gun.Core.Tests
{
    public class DuplicateManagerTest
    {
        [Fact]
        public void ShouldRandom()
        {
            var res = DuplicateManager.Random();
            Assert.Equal(3, res.Length);
        }

        [Fact]
        public void ShouldTrack()
        {
            var dup = new DuplicateManager(Options.Create<DuplicateManagerOptions>(new DuplicateManagerOptions()));
            var key = DuplicateManager.Random();
            var res = dup.Track(key);
            Assert.Equal(key,res);
        }

        [Fact]
        public void ShouldCheckFalseWithNotExist()
        {
            var dup = new DuplicateManager(Options.Create<DuplicateManagerOptions>(new DuplicateManagerOptions()));
            var key = DuplicateManager.Random();
            var res = dup.Check(key);
            Assert.False(false);
        }

        [Fact]
        public void ShouldCheckTrueWithDuplicate()
        {
            var dup = new DuplicateManager(Options.Create<DuplicateManagerOptions>(new DuplicateManagerOptions()));
            var key = DuplicateManager.Random();
            var res = dup.Check(key);
            var res2 = dup.Track(key);
            var res3 = dup.Check(key);
            Assert.False(res);
            Assert.True(res3);
        }

        [Fact]
        public async void ShouldCheckFalseAfterTimeout()
        {
            var dup = new DuplicateManager(Options.Create<DuplicateManagerOptions>(new DuplicateManagerOptions()));
            var key = DuplicateManager.Random();
            var res = dup.Check(key);
            var res2 = dup.Track(key);
            await Task.Delay(1000 * 10);
            var res3 = dup.Check(key);
            Assert.False(res);
            Assert.False(res3);
        }

    }
}
