﻿using System;
using System.Collections.Generic;
using FluentAssertions;
using GoC.WebTemplate;
using GoC.WebTemplate.Proxies;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Ploeh.AutoFixture.Xunit2;
using WebTemplateCore.JSONSerializationObjects;
using Xunit;

namespace CoreTest
{
    public class PrefooterSerializationTests
    {
        [Fact]
        public void RenderShowShareAsList()
        {
            var preFooter = new PreFooter
            {
                ShowShare = new ShareList
                {
                    Show = true,
                    Enums = new List<Core.SocialMediaSites>
                    {
                        Core.SocialMediaSites.bitly, Core.SocialMediaSites.blogger,
                    }
                }
            };
            var json = JsonSerializationHelper.SerializeToJson(preFooter);
            json.ToString().Should().Contain("\"showShare\":[\"bitly\",\"blogger\"]");
        }
        [Fact]
        public void RenderShowShareAsTrue()
        {
            var preFooter = new PreFooter
            {
                ShowShare = new ShareList
                {
                    Show = true,
                    Enums = new List<Core.SocialMediaSites>()
                }
            };
            var json = JsonSerializationHelper.SerializeToJson(preFooter);
            json.ToString().Should().Contain("\"showShare\":true");
        }

        [Fact]
        public void RenderShowShareAsFalse()
        {
            var preFooter = new PreFooter
            {
                ShowShare = new ShareList
                {
                    Show = false,
                    Enums = new List<Core.SocialMediaSites>()
                }
            };
            var json = JsonSerializationHelper.SerializeToJson(preFooter);
            json.ToString().Should().Contain("\"showShare\":false");
            
        }
        [Fact]
        public void RenderShowFeedbackAsTrue()
        {
            var preFooter = new PreFooter
            {
                ShowFeedback = new FeedbackLink
                {
                    Show = true,
                    URL = string.Empty
                }
            };
            var json = JsonSerializationHelper.SerializeToJson(preFooter);
            json.ToString().Should().Contain("\"showFeedback\":true");
        } 

        [Fact]
        public void RenderShowFeedbackAsURL()
        {
            var preFooter = new PreFooter
            {
                ShowFeedback = new FeedbackLink
                {
                    Show = true,
                    URL = "foo"
                }
            };
            var json = JsonSerializationHelper.SerializeToJson(preFooter);
            json.ToString().Should().Contain("\"showFeedback\":\"foo\"");
        }

        [Fact]
        public void RenderShowFeedbackAsFalse()
        {
            var preFooter = new PreFooter
            {
                ShowFeedback = new FeedbackLink
                {
                    Show = false,
                    URL = "foo"
                }
            };
            var json = JsonSerializationHelper.SerializeToJson(preFooter);
            json.ToString().Should().Contain("\"showFeedback\":false");
        }

    }
    /// <summary>
    /// Tests that test the Core object in isolation.
    /// </summary>
    public class RenderTests 
    {

        [Theory, AutoNSubstituteData]
        public void DoNotRenderBreadCrumbsByDefault(Core sut)
        {
            sut.RenderTop().ToString().Should().NotContain("\"breadcrumbs\"");
        }

        [Theory, AutoNSubstituteData]
        public void RenderCustomSearchWhenSet(Core sut)
        {
            sut.CustomSearch="Foo";
            sut.RenderAppTop().ToString().Should().Contain("\"customSearch\":\"Foo\"");
        }

        [Theory, AutoNSubstituteData]
        public void LocalPathDoesNotRendersWhenNull([Frozen]IDictionary<string, ICDTSEnvironment> environments,
            [Frozen]IConfigurationProxy proxy, Core sut)
        {
            environments[sut.Environment].LocalPath.ReturnsNull();
            sut.RenderRefTop().ToString().Should().NotContain("localPath");
        }

        [Theory, AutoNSubstituteData]
        public void LocalPathFormatsCorrectly([Frozen]IDictionary<string, ICDTSEnvironment> environments,
            [Frozen]IConfigurationProxy proxy,
            Core sut)
        {
            environments[sut.Environment].LocalPath.Returns("{0}:{1}");
            sut.RenderRefTop().ToString().Should().Contain($"\"localPath\":\"{sut.WebTemplateTheme}:{sut.WebTemplateVersion}");
        }


        public void LocalPathRendersWhenNotNull(Core sut)
        {
            sut.RenderRefTop().ToString().Should().Contain("localPath");
        }
        [Theory, AutoNSubstituteData]
        public void JQueryExternalRendersWhenLoadJQueryFromGoogleIsTrue(Core sut)
        {
            sut.LoadJQueryFromGoogle = true;

            sut.RenderRefTop().ToString().Should().Contain("\"jqueryEnv\":\"external\"");
        }
        [Theory, AutoNSubstituteData]
        public void JQueryExternalDoesNotRenderWhenLoadJQueryFromGoogleIsFalse(Core sut)
        {
            sut.LoadJQueryFromGoogle = false;

            sut.RenderRefTop().ToString().Should().NotContain("jqueryEnv");
        }

        [Theory, AutoNSubstituteData]
        public void WebSubThemeRenderedProperly(Core sut)
        {

            sut.RenderRefTop().ToString().Should().Contain($"\"subTheme\":\"{sut.WebTemplateSubTheme}\"");
        }
        /*
        //Current different types of environments
              "https://www.canada.ca/etc/designs/canada/cdts/GCWeb/{0}/cdts/compiled/",
              "https://ssl-templates.services.gc.ca/{1}/cls/wet/GCIntranet/{3}cdts/compiled/",
              "Path": "http{0}://templates.service.gc.ca/{1}/cls/wet/GCIntranet/{3}cdts/compiled/",
              "Path": "http{0}://s2tst-cdn-canada.sade-edap.prv/{1}/cls/wet/{2}/{3}cdts/compiled/",
        */
        [Theory, AutoNSubstituteData]
        public void CdnEnvRenderedProperly([Frozen]IDictionary<string, ICDTSEnvironment> environments, 
            Core sut)
        {
            environments[sut.Environment].CDN = "prod";
            sut.RenderRefTop().ToString().Should().Contain("\"cdnEnv\":\"prod\"");
        }

        [Theory, AutoNSubstituteData]
        public void ShouldNotEncodeURL(Core sut)
        {
            sut.ContactLinkURL = "http://localhost:8080/foo.html";
            var htmlstring = sut.RenderFooterLinks(false);
            htmlstring.ToString().Should().Contain("http://localhost:8080/foo.html");
        }

        [Theory, AutoNSubstituteData]
        public void CustomSearchIsRendered(Core sut)
        {
            sut.CustomSearch = "foo";
            var json = sut.RenderAppTop();
            json.ToString().Should().Contain("\"customSearch\":\"foo\"");

        }

        [Theory, AutoNSubstituteData]
        public void ExceptionWhenCallingRenderFooterLinks(Core sut)
        {
            sut.ContactLinkURL = null;
            Action execute = () => sut.RenderFooterLinks(false);
            execute.ShouldNotThrow<NullReferenceException>();

        }

        [Theory, AutoNSubstituteData]
        public void HandleEmptyContactLinkList(Core sut)
        {
            sut.ContactLinkURL = "bar";
            sut.RenderAppFooter().ToString().Should().Contain("\"contactLinks\":[{\"href\":\"bar\"}]");
        }

        [Theory, AutoNSubstituteData]
        public void RenderAppTopMustNotCrashWithNullBreadCrumbs(Core sut)
        {
            sut.Breadcrumbs = null;
            // ReSharper disable once MustUseReturnValue
            Action execute = () => sut.RenderAppTop();
            execute.ShouldNotThrow<ArgumentNullException>();
        }
        [Theory, AutoNSubstituteData]
        public void DoNotAddCanadaCaToTitlesIfItIsAlreadyThere(Core sut)
        {
            sut.HeaderTitle = "Foo - Canada.ca";
            sut.WebTemplateTheme = "GCWeb";
            sut.HeaderTitle.Should().Be("Foo - Canada.ca");
        }
        [Theory, AutoNSubstituteData]
        public void AddCanadaCaToAllTitlesOnPagesImplementingGCWebTheme(Core sut)
        {
            sut.HeaderTitle = "Foo";
            sut.WebTemplateTheme = "GCWeb";
            sut.HeaderTitle.Should().Be("Foo - Canada.ca");
        }

        [Theory, AutoNSubstituteData]
        public void AddCanadaCaToAllTitlesOnPagesWhenTitleIsNullImplementingGCWebTheme(Core sut)
        {
            sut.HeaderTitle = null;
            sut.WebTemplateTheme = "GCWeb";
            sut.HeaderTitle.Should().Be("- Canada.ca");
        }
        [Theory, AutoNSubstituteData]
        public void DontAddCanadaCaToAllTitlesOnPagesImplementingGCWebTheme(Core sut)
        {
            sut.HeaderTitle = "Foo";
            sut.WebTemplateTheme = "Intranet";
            sut.HeaderTitle.Should().Be("Foo");
        }
        [Theory, AutoNSubstituteData]
        public void SiteMenuPathShouldNotRenderWhenNull(Core sut)
        {
            var json = sut.RenderAppTop();
            json.ToString().Should().NotContain("menuPath");
        }


        [Theory, AutoNSubstituteData]
        public void PrivacyLinkNotRenderedWhenURLIsNull(Core sut)
        {
            sut.PrivacyLinkURL = null;
            var json = sut.RenderAppFooter();
            json.ToString().Should().NotContain("privacyLink");
        }

        [Theory, AutoNSubstituteData]
        public void PrivacyLinkRenderedWhenURLIsProvided(Core sut)
        {
            sut.PrivacyLinkURL = "http://foo.bar";
            var json = sut.RenderAppFooter();
            json.ToString().Should().Contain("privacyLink");
        }


        [Theory, AutoNSubstituteData]
        public void TermsLinkNotRenderedWhenURLIsNull(Core sut)
        {
            sut.TermsConditionsLinkURL = null;
            var json = sut.RenderAppFooter();
            json.ToString().Should().NotContain("termsLink");
        }

        [Theory, AutoNSubstituteData]
        public void TermsLinkRenderedWhenURLIsProvided(Core sut)
        {
            sut.TermsConditionsLinkURL = "http://foo.bar";
            var json = sut.RenderAppFooter();
            json.ToString().Should().Contain("termsLink");
        }

        [Theory, AutoNSubstituteData]
        public void SignInLinkNotRenderedWhenFlagisFalse(Core sut)
        {

            sut.ShowSignInLink = false;
            var json = sut.RenderAppTop();
            json.ToString().Should().NotContain("signIn");
        }

        [Theory, AutoNSubstituteData]
        public void SignInLinkNotRenderedWhenLinkIsNull(Core sut)
        {

            sut.ShowSignInLink = true;
            sut.SignInLinkURL = null;
            var json = sut.RenderAppTop();
            json.ToString().Should().NotContain("signIn");
        }

        [Theory, AutoNSubstituteData]
        public void SignOutLinkNotRenderedWhenFlagisFalse(Core sut)
        {

            sut.ShowSignOutLink = false;
            var json = sut.RenderAppTop();
            json.ToString().Should().NotContain("signOut");
        }

        [Theory, AutoNSubstituteData]
        public void SignOutLinkNotRenderedWhenLinkIsNull(Core sut)
        {

            sut.ShowSignOutLink = true;
            sut.ShowSignInLink = false;
            sut.SignOutLinkURL = null;
            var json = sut.RenderAppTop();
            json.ToString().Should().NotContain("signOut");
        }

        [Theory, AutoNSubstituteData]
        public void SignInAndSignOutLinkBothOn(Core sut)
        {
            sut.ShowSignOutLink = true;
            sut.ShowSignInLink = true;
            // ReSharper disable once MustUseReturnValue
            Action act = () =>  sut.RenderAppTop();
            act.ShouldThrow<InvalidOperationException>();
        }

        [Theory, AutoNSubstituteData]
        public void RenderLeftMenuTest( Core sut)
        {
            sut.LeftMenuItems.Add(new MenuSection("SectionName", "SectionLink", new[] {new Link("Href", "Text")}));

            var result = sut.RenderLeftMenu();

            result.ToString().Should().Be("sections: [ {sectionName: 'SectionName', sectionLink: 'SectionLink', menuLinks: [{href: 'Href', text: 'Text'},]},]");
        }

        [Theory, AutoNSubstituteData]
        public void RenderEmptyLeftMenu(Core sut)
        {
            var result = sut.RenderLeftMenu();
            result.ToString().Should().BeEmpty();
        }


    }
}
