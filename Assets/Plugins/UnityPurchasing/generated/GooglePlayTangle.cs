#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("zIgTmJsw9rjBsy3VA7qbZ42DgTrv8Sxp73GujPXJnVTpCUKvBrIyYjZaRpPkOKLdnZnfHWEIiDuB5kwZELoP6rhFbv+1SGV0mo+ZzTNHZk/iHEZTpiw7eRvPygMD5ZP1+/2BqrXeoRkaS4oERf9pVGTZ80Lo1RXeuApDN8z+ripDwuzrB7NmJ70i3+WkUwWTD07XgCtgopPDEGrtAdywHnj79frKePvw+Hj7+/p+N1y8TYO6ThSW4r07T/ta0R6isUQ1lwQdDPO+LQBwzUcvDY5+oqK201OOZZ+tLsp4+9jK9/zz0HyyfA33+/v7//r5kmmk9QRAzoeEh7SqWg0lzAy/l4LASzjThvben9nuo7dkNy0MzbqPkO30GmOBXHjPzfj5+/r7");
        private static int[] order = new int[] { 6,11,5,8,13,8,10,9,11,13,13,13,13,13,14 };
        private static int key = 250;

        public static byte[] Data() {
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif
