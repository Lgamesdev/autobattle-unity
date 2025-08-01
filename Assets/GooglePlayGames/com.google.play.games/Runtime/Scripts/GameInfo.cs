// <copyright file="GameInfo.cs" company="Google Inc.">
// Copyright (C) 2015 Google Inc. All Rights Reserved.
//
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//
//  http://www.apache.org/licenses/LICENSE-2.0
//
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//    limitations under the License.
// </copyright>
#if UNITY_ANDROID

namespace GooglePlayGames {
    ///
    /// This file is automatically generated DO NOT EDIT!
    ///
    /// These are the constants defined in the Play Games Console for Game Services
    /// Resources.
    ///
    /// <summary>
    /// File containing information about the game. This is automatically updated by running the
    /// platform-appropriate setup commands in the Unity editor (which does a simple search / replace
    /// on the IDs in the form "__ID__"). We can check whether any particular field has been updated
    /// by checking whether it still retains its initial value - we prevent the constants from being
    /// replaced in the aforementioned search/replace by stripping off the leading and trailing "__".
    /// </summary>
    public static class GameInfo {

        private const string UnescapedApplicationId = "APP_ID";
        private const string UnescapedIosClientId = "IOS_CLIENTID";
        private const string UnescapedWebClientId = "WEB_CLIENTID";
        private const string UnescapedNearbyServiceId = "NEARBY_SERVICE_ID";

        public const string ApplicationId = "79826388381"; // Filled in automatically
        public const string IosClientId = "__IOS_CLIENTID__"; // Filled in automatically
        public const string WebClientId = "79826388381-aikta20ttsmvsv1pqok3ar6pvltrojdr.apps.googleusercontent.com"; // Filled in automatically
        public const string NearbyConnectionServiceId = "";

        public static bool ApplicationIdInitialized() {
            return !string.IsNullOrEmpty(ApplicationId) && !ApplicationId.Equals(ToEscapedToken(UnescapedApplicationId));
        }

        public static bool IosClientIdInitialized() {
            return !string.IsNullOrEmpty(IosClientId) && !IosClientId.Equals(ToEscapedToken(UnescapedIosClientId));
        }

        public static bool WebClientIdInitialized() {
            return !string.IsNullOrEmpty(WebClientId) && !WebClientId.Equals(ToEscapedToken(UnescapedWebClientId));
        }

        public static bool NearbyConnectionsInitialized() {
            return !string.IsNullOrEmpty(NearbyConnectionServiceId) &&
             !NearbyConnectionServiceId.Equals(ToEscapedToken(UnescapedNearbyServiceId));
        }

        /// <summary>
        /// Returns an escaped token (i.e. one flanked with "__") for the passed token
        /// </summary>
        /// <returns>The escaped token.</returns>
        /// <param name="token">The Token</param>
        private static string ToEscapedToken(string token) {
            return string.Format("__{0}__", token);
        }
    }
}
#endif
