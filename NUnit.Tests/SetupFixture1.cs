﻿using NUnit.Framework;
using System;

namespace NUnit.Tests
{
    [SetUpFixture]
    public class SetupFixture1
    {
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            // TODO: Add code here that is run before
            //  all tests in the assembly are run            
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            // TODO: Add code here that is run after
            //  all tests in the assembly have been run
        }
    }
}