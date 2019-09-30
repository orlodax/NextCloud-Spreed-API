using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace NextCloudAPI.Models
{
    public class UserInfo
    {
        public string storageLocation { get; set; } = string.Empty;
        public string id { get; set; } = string.Empty;
        public ulong lasLogin { get; set; }
        public string backend { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string displayname { get; set; } = string.Empty;
        public string phone { get; set; } = string.Empty;
        public string address { get; set; } = string.Empty;
        public string website { get; set; } = string.Empty;
        public string twitter { get; set; } = string.Empty;
        public string language { get; set; } = string.Empty;

        ///ui
        public Bitmap Avatar { get; set; }
    }
}



///esempio di risposta
///
//<data>
//        <storageLocation>/var/www/nextcloud/data/Dario</storageLocation>
//        <id>Dario</id>
//        <lastLogin>1569832026000</lastLogin>
//        <backend>Database</backend>
//        <subadmin/>
//        <quota>
//            <free>1545137446912</free>
//            <used>12896168</used>
//            <total>1545150343080</total>
//            <relative>0</relative>
//            <quota>-3</quota>
//        </quota>
//        <email/>
//        <displayname>Dario Orlovich</displayname>
//        <phone></phone>
//        <address></address>
//        <website></website>
//        <twitter></twitter>
//        <groups>
//            <element>Teksistemi</element>
//        </groups>
//        <language>en</language>
//        <locale></locale>
//        <backendCapabilities>
//            <setDisplayName>1</setDisplayName>
//            <setPassword>1</setPassword>
//        </backendCapabilities>
//    </data>
