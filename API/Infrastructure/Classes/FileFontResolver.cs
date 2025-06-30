using System;
using System.IO;
using PdfSharp.Fonts;

namespace API.Infrastructure.Classes {

    public class FileFontResolver : IFontResolver {

        public string DefaultFontName => throw new NotImplementedException();

        public byte[] GetFont(string faceName) {
            using var ms = new MemoryStream();
            using var fs = File.Open(faceName, FileMode.Open);
            fs.CopyTo(ms);
            ms.Position = 0;
            return ms.ToArray();
        }

        public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic) {
            return familyName switch {
                "ACCanterBold" => new FontResolverInfo("Fonts/ACCanterBold.ttf"),
                "RobotoMono" => new FontResolverInfo("Fonts/RobotoMono.ttf"),
                "MonoType" => new FontResolverInfo("Fonts/MonoType.ttf"),
                _ => new FontResolverInfo("Fonts/Tahoma.ttf")
            };
        }

    }

}