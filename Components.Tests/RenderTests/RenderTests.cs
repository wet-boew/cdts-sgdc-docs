using System.Collections.Generic;
using AutoFixture.Xunit2;
using FluentAssertions;
using GoC.WebTemplate.Components.Configs;
using GoC.WebTemplate.Components.Configs.Cdts;
using GoC.WebTemplate.Components.Entities;
using GoC.WebTemplate.Components.Utils.Caching;
using NSubstitute;
using Xunit;

namespace GoC.WebTemplate.Components.Test.RenderTests
{
    /// <summary>
    /// Tests that test the Model object in isolation.
    /// </summary>
    public class RenderTests
    {

        [Theory, AutoNSubstituteData]
        public void DoNotAddCanadaCaToTitlesIfItIsAlreadyThere([Frozen] ICdtsCacheProvider cdtsCacheProvider,
            ICdtsEnvironment env,
            Model sut)
        {

            env.AppendToTitle.Returns(" - Canada.ca");
            new CdtsEnvironmentCache(cdtsCacheProvider).GetContent()[sut.Settings.Environment] = env;

            sut.HeaderTitle = "Foo - Canada.ca";
            sut.HeaderTitle.Should().Be("Foo - Canada.ca");
        }

        [Theory, AutoNSubstituteData]
        public void AddCanadaCaToAllTitlesOnPagesImplementingGCWebTheme([Frozen]ICdtsCacheProvider cdtsCacheProvider, 
            ICdtsEnvironment env, Model sut)
        {
            env.AppendToTitle.Returns(" - Canada.ca");
            new CdtsEnvironmentCache(cdtsCacheProvider).GetContent()[sut.Settings.Environment] = env;
            sut.HeaderTitle = "Foo";

            sut.HeaderTitle.Should().Be("Foo - Canada.ca");
        }

        [Theory, AutoNSubstituteData]
        public void AddCanadaCaToAllTitlesOnPagesWhenTitleIsNullImplementingGCWebTheme([Frozen]ICdtsCacheProvider cdtsCacheProvider, 
            ICdtsEnvironment env, Model sut)
        {
            env.AppendToTitle.Returns(" - Canada.ca");
            new CdtsEnvironmentCache(cdtsCacheProvider).GetContent()[sut.Settings.Environment] = env;

            sut.HeaderTitle = null;

            sut.HeaderTitle.Should().Be(" - Canada.ca");
        }

        [Theory, AutoNSubstituteData]
        public void DontAddCanadaCaToAllTitlesOnPagesImplementingGCWebTheme([Frozen] ICdtsCacheProvider cdtsCacheProvider,
            ICdtsEnvironment env,
            Model sut)
        {
            env.AppendToTitle.Returns("");
            new CdtsEnvironmentCache(cdtsCacheProvider).GetContent()[sut.Settings.Environment] = env;

            sut.HeaderTitle = "Foo";

            sut.HeaderTitle.Should().Be("Foo");
        }

        [Theory, AutoNSubstituteData]
        public void RenderLeftMenu(Model sut)
        {
            sut.LeftMenuItems.Add(new MenuSection
            {
                Text = "SectionName",
                Href = "SectionLink",
                Items = new List<MenuItem> {
                    new MenuItem { Href = "Href", Text = "Text" },
                    new MenuItem {
                        Href = "Href",
                        Text = "Text",
                        SubItems = new List<Link> {
                            new Link { Href = "subHerf", Text = "subText" }
                        }
                    },
                    new MenuItem { Href = "Herf", Text = "Text", NewWindow = true }
                }
            });
            var result = sut.Render.Setup();

            result.ToString().Should().Contain("{\"sections\":[{\"sectionName\":\"SectionName\",\"sectionLink\":\"SectionLink\",\"menuLinks\":[{\"href\":\"Href\",\"text\":\"Text\"},{\"href\":\"Href\",\"text\":\"Text\",\"subLinks\":[{\"subhref\":\"subHerf\",\"subtext\":\"subText\"}]},{\"href\":\"Herf\",\"text\":\"Text\",\"newWindow\":true}]}]}");
        }

        [Theory, AutoNSubstituteData]
        public void RenderEmptyLeftMenu(Model sut)
        {
            var result = sut.Render.Setup();
            result.ToString().Should().NotContain("sectionName");
        }

        [Theory, AutoNSubstituteData]
        public void RenderSessionTimeoutControl(Model sut)
        {
            sut.Settings.SessionTimeout.Enabled = true;

            // AutoFixture sets this to false by default, but that value isn't 
            // emitted during JSON serialization because it's a default value 
            // on an optional property. LogoutUrl, a required property, would also
            // fail this test if it wasn't automatically set by AutoFixture.
            //TODO: If we intend to test for all values being emitted, even optional ones, we should set them explicitly as part of the scenario. Not with AutoFixture auto-properties.
            sut.Settings.SessionTimeout.RefreshOnClick = true;

            var result = sut.Render.SessionTimeoutControl();
            result.ToString().Should().ContainAll("class='wb-sessto'", "inactivity", "reactionTime", "sessionalive", "logouturl", "refreshCallbackUrl", "refreshOnClick", "refreshLimit", "method", "additionalData");
        }

        [Theory, AutoNSubstituteData]
        public void RenderSessionTimeoutControlTextOverrides(Model sut)
        {
            sut.Settings.SessionTimeout.Enabled = true;
            sut.Settings.SessionTimeout.RefreshOnClick = true;
            sut.Settings.SessionTimeout.TextOverrides = new WebTemplate.Entities.Source.SessionTimeoutTextOverrides
            {
                ButtonContinue = "Continue Button",
                ButtonEnd = "End Button",
                ButtonSignIn = "Sign In Button",
                TimeoutAlready = "Timeout Already",
                TimeoutEnd = "Timeout End"
            };
            var result = sut.Render.SessionTimeoutControl();
            result.ToString().Should().ContainAll("class='wb-sessto'", "inactivity", "reactionTime", "sessionalive", "logouturl", "refreshCallbackUrl", "refreshOnClick", "refreshLimit", "method", "additionalData", "textOverrides", "buttonContinue", "buttonEnd", "buttonSignin", "timeoutAlready", "timeoutEnd");
        }


        [Theory, AutoNSubstituteData]
        public void RenderSessionRefreshRendersDefaultValue(Model sut)
        {
            sut.Settings.SessionTimeout.RefreshOnClick = default;

            var result = sut.Render.SessionTimeoutControl();
            result.ToString().Should().Contain("\"refreshOnClick\":false");
        }

        [Theory, AutoNSubstituteData]
        public void RenderHeaderTitle(Model sut)
        {
            sut.HeaderTitle = "MasterfullHeaderTitle";
            var result = sut.Render.HeaderTitle();
            result.ToString().Should().Be("MasterfullHeaderTitle");
        }

        [Theory, AutoNSubstituteData]
        public void RenderHtmlBodyElementsEmpty(Model sut)
        {
            var result = sut.Render.Setup();
            result.ToString().Should().Contain("\"onCDTSPageFinalized\":[]");
        }

        [Theory, AutoNSubstituteData]
        public void RenderHtmlHeaderElementsEmpty(Model sut)
        {
            var result = sut.Render.HtmlHeaderElements();
            result.ToString().Should().BeEmpty();
        }

        [Theory, AutoNSubstituteData]
        public void RenderHtmlBodyElements(Model sut)
        {
            sut.HTMLBodyElements = new List<string> { "Fake Body Element", "Other Item" };
            var result = sut.Render.Setup();
            result.ToString().Should().Contain("\"Fake Body Element\",\"Other Item\"");
        }

        [Theory, AutoNSubstituteData]
        public void RenderHtmlHeaderElements(Model sut)
        {
            sut.HTMLHeaderElements = new List<string> { "Fake Header Element", "Other Item" };
            var result = sut.Render.HtmlHeaderElements();
            result.ToString().Should().Be("Fake Header Element\r\nOther Item\r\n");
        }
    }
}
