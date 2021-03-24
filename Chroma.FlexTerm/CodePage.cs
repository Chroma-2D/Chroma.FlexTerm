﻿using System.Collections.Generic;

namespace Chroma.FlexTerm
{
    internal static class CodePage
    {
        private static List<char> Cp437 = new()
        {
            '\u263A', '\u263B', '\u2665', '\u2666', '\u2663', '\u2660', '\u2022', '\u25D8',
            '\u25CB', '\u25D9', '\u2642', '\u2640', '\u266A', '\u266B', '\u263C', '\u25BA',
            '\u25C4', '\u2195', '\u203C', '\u00B6', '\u00A7', '\u25AC', '\u21A8', '\u2191',
            '\u2193', '\u2192', '\u2190', '\u221F', '\u2194', '\u25B2', '\u25BC', '\u0021',
            '\u0022', '\u0023', '\u0024', '\u0025', '\u0026', '\u0027', '\u0028', '\u0029',
            '\u002A', '\u002B', '\u002C', '\u002D', '\u002E', '\u002F', '\u0030', '\u0031',
            '\u0032', '\u0033', '\u0034', '\u0035', '\u0036', '\u0037', '\u0038', '\u0039',
            '\u003A', '\u003B', '\u003C', '\u003D', '\u003E', '\u003F', '\u0040', '\u0041',
            '\u0042', '\u0043', '\u0044', '\u0045', '\u0046', '\u0047', '\u0048', '\u0049',
            '\u004A', '\u004B', '\u004C', '\u004D', '\u004E', '\u004F', '\u0050', '\u0051',
            '\u0052', '\u0053', '\u0054', '\u0055', '\u0056', '\u0057', '\u0058', '\u0059',
            '\u005A', '\u005B', '\u005C', '\u005D', '\u005E', '\u005F', '\u0060', '\u0061',
            '\u0062', '\u0063', '\u0064', '\u0065', '\u0066', '\u0067', '\u0068', '\u0069',
            '\u006A', '\u006B', '\u006C', '\u006D', '\u006E', '\u006F', '\u0070', '\u0071',
            '\u0072', '\u0073', '\u0074', '\u0075', '\u0076', '\u0077', '\u0078', '\u0079',
            '\u007A', '\u007B', '\u007C', '\u007D', '\u007E', '\u2302', '\u00C7', '\u00FC',
            '\u00E9', '\u00E2', '\u00E4', '\u00E0', '\u00E5', '\u00E7', '\u00EA', '\u00EB',
            '\u00E8', '\u00EF', '\u00EE', '\u00EC', '\u00C4', '\u00C5', '\u00C9', '\u00E6',
            '\u00C6', '\u00F4', '\u00F6', '\u00F2', '\u00FB', '\u00F9', '\u00FF', '\u00D6',
            '\u00DC', '\u00A2', '\u00A3', '\u00A5', '\u20A7', '\u0192', '\u00E1', '\u00ED',
            '\u00F3', '\u00FA', '\u00F1', '\u00D1', '\u00AA', '\u00BA', '\u00BF', '\u2310',
            '\u00AC', '\u00BD', '\u00BC', '\u00A1', '\u00AB', '\u00BB', '\u2591', '\u2592',
            '\u2593', '\u2502', '\u2524', '\u2561', '\u2562', '\u2556', '\u2555', '\u2563',
            '\u2551', '\u2557', '\u255D', '\u255C', '\u255B', '\u2510', '\u2514', '\u2534',
            '\u252C', '\u251C', '\u2500', '\u253C', '\u255E', '\u255F', '\u255A', '\u2554',
            '\u2569', '\u2566', '\u2560', '\u2550', '\u256C', '\u2567', '\u2568', '\u2564',
            '\u2565', '\u2559', '\u2558', '\u2552', '\u2553', '\u256B', '\u256A', '\u2518',
            '\u250C', '\u2588', '\u2584', '\u258C', '\u2590', '\u2580', '\u0251', '\u03D0',
            '\u1D26', '\u1D28', '\u2211', '\u01A1', '\u00B5', '\u1D1B', '\u0278', '\u03F4',
            '\u2126', '\u1E9F', '\u221E', '\u2205', '\u2208', '\u2229', '\u2261', '\u00B1',
            '\u2265', '\u2264', '\u2320', '\u2321', '\u00F7', '\u2248', '\u00B0', '\u2219',
            '\u00B7', '\u221A', '\u207F', '\u00B2', '\u25A0', '\u0020'
        };

        private static List<char> Cp437Plus = new()
        {
            '\u0021', '\u0022', '\u0023', '\u0024', '\u0025', '\u0026', '\u0027', '\u0028',
            '\u0029', '\u002A', '\u002B', '\u002C', '\u002D', '\u002E', '\u002F', '\u0030',
            '\u0031', '\u0032', '\u0033', '\u0034', '\u0035', '\u0036', '\u0037', '\u0038',
            '\u0039', '\u003A', '\u003B', '\u003C', '\u003D', '\u003E', '\u003F', '\u0040',
            '\u0041', '\u0042', '\u0043', '\u0044', '\u0045', '\u0046', '\u0047', '\u0048',
            '\u0049', '\u004A', '\u004B', '\u004C', '\u004D', '\u004E', '\u004F', '\u0050',
            '\u0051', '\u0052', '\u0053', '\u0054', '\u0056', '\u0057', '\u0058',
            '\u0059', '\u005A', '\u005B', '\u005C', '\u005D', '\u005E', '\u005F', '\u0060',
            '\u0061', '\u0062', '\u0063', '\u0064', '\u0065', '\u0066', '\u0067', '\u0068',
            '\u0069', '\u006A', '\u006B', '\u006C', '\u006D', '\u006E', '\u006F', '\u0070',
            '\u0071', '\u0072', '\u0073', '\u0074', '\u0075', '\u0076', '\u0077', '\u0078',
            '\u0079', '\u007A', '\u007B', '\u007C', '\u007D', '\u007E', '\u0020',
            '\u00A1', '\u00A2', '\u00A3', '\u00A4', '\u00A5', '\u00A6', '\u00A7', '\u00A8',
            '\u00A9', '\u00AA', '\u00AB', '\u00AC', '\u00AE', '\u00AF', '\u00B0',
            '\u00B1', '\u00B2', '\u00B3', '\u00B4', '\u00B5', '\u00B6', '\u00B7', '\u00B8',
            '\u00B9', '\u00BA', '\u00BB', '\u00BC', '\u00BD', '\u00BE', '\u00BF', '\u00C0',
            '\u00C1', '\u00C2', '\u00C3', '\u00C4', '\u00C5', '\u00C6', '\u00C7', '\u00C8',
            '\u00C9', '\u00CA', '\u00CB', '\u00CC', '\u00CD', '\u00CE', '\u00CF', '\u00D0',
            '\u00D1', '\u00D2', '\u00D3', '\u00D4', '\u00D5', '\u00D6', '\u00D7', '\u00D8',
            '\u00D9', '\u00DA', '\u00DB', '\u00DC', '\u00DD', '\u00DE', '\u00DF', '\u00E0',
            '\u00E1', '\u00E2', '\u00E3', '\u00E4', '\u00E5', '\u00E6', '\u00E7', '\u00E8',
            '\u00E9', '\u00EA', '\u00EB', '\u00EC', '\u00ED', '\u0055',
            '\u00EE', '\u00EF', '\u00F0', '\u00F1', '\u00F2',
            '\u00F3', '\u00F4', '\u00F5', '\u00F6', '\u00F7', '\u00F8', '\u00F9', '\u00FA',
            '\u00FB', '\u00FC', '\u00FD', '\u00FE', '\u00FF', '\u0100', '\u0101', '\u0102',
            '\u0103', '\u0104', '\u0105', '\u0106', '\u0107', '\u0108', '\u0109', '\u010A',
            '\u010B', '\u010C', '\u010D', '\u010E', '\u010F', '\u0110', '\u0111', '\u0112',
            '\u0113', '\u0114', '\u0115', '\u0116', '\u0117', '\u0118', '\u0119', '\u011A',
            '\u011B', '\u011C', '\u011D', '\u011E', '\u011F', '\u0120', '\u0121', '\u0122',
            '\u0123', '\u0124', '\u0125', '\u0126', '\u0127', '\u0128', '\u0129', '\u012A',
            '\u012B', '\u012C', '\u012D', '\u012E', '\u012F', '\u0130', '\u0131', '\u0132',
            '\u0133', '\u0134', '\u0135', '\u0136', '\u0137', '\u0138', '\u0139', '\u013A',
            '\u013B', '\u013C', '\u013D', '\u013E', '\u013F', '\u0140', '\u0141', '\u0142',
            '\u0143', '\u0144', '\u0145', '\u0146', '\u0147', '\u0148', '\u0149', '\u014A',
            '\u014B', '\u014C', '\u014D', '\u014E', '\u014F', '\u0150', '\u0151', '\u0152',
            '\u0153', '\u0154', '\u0155', '\u0156', '\u0157', '\u0158', '\u0159', '\u015A',
            '\u015B', '\u015C', '\u015D', '\u015E', '\u015F', '\u0160', '\u0161', '\u0162',
            '\u0163', '\u0164', '\u0165', '\u0166', '\u0167', '\u0168', '\u0169', '\u016A',
            '\u016B', '\u016C', '\u016D', '\u016E', '\u016F', '\u0170', '\u0171', '\u0172',
            '\u0173', '\u0174', '\u0175', '\u0176', '\u0177', '\u0178', '\u0179', '\u017A',
            '\u017B', '\u017C', '\u017D', '\u017E', '\u017F', '\u0192', '\u01A1', '\u01B7',
            '\u01FA', '\u01FB', '\u01FC', '\u01FD', '\u01FE', '\u01FF', '\u0218', '\u0219',
            '\u021A', '\u021B', '\u0251', '\u0278', '\u02C6', '\u02C7', '\u02C9', '\u02D8',
            '\u02D9', '\u02DA', '\u02DB', '\u02DC', '\u02DD', '\u037E', '\u0384', '\u0385',
            '\u0386', '\u0387', '\u0388', '\u0389', '\u038A', '\u038C', '\u038E', '\u038F',
            '\u0390', '\u0391', '\u0392', '\u0393', '\u0394', '\u0395', '\u0396', '\u0397',
            '\u0398', '\u0399', '\u039A', '\u039B', '\u039C', '\u039D', '\u039E', '\u039F',
            '\u03A0', '\u03A1', '\u03A3', '\u03A4', '\u03A5', '\u03A6', '\u03A7', '\u03A8',
            '\u03A9', '\u03AA', '\u03AB', '\u03AC', '\u03AD', '\u03AE', '\u03AF', '\u03B0',
            '\u03B1', '\u03B2', '\u03B3', '\u03B4', '\u03B5', '\u03B6', '\u03B7', '\u03B8',
            '\u03B9', '\u03BA', '\u03BB', '\u03BC', '\u03BD', '\u03BE', '\u03BF', '\u03C0',
            '\u03C1', '\u03C2', '\u03C3', '\u03C4', '\u03C5', '\u03C6', '\u03C7', '\u03C8',
            '\u03C9', '\u03CA', '\u03CB', '\u03CC', '\u03CD', '\u03CE', '\u03D0', '\u03F4',
            '\u0400', '\u0401', '\u0402', '\u0403', '\u0404', '\u0405', '\u0406', '\u0407',
            '\u0408', '\u0409', '\u040A', '\u040B', '\u040C', '\u040D', '\u040E', '\u040F',
            '\u0410', '\u0411', '\u0412', '\u0413', '\u0414', '\u0415', '\u0416', '\u0417',
            '\u0418', '\u0419', '\u041A', '\u041B', '\u041C', '\u041D', '\u041E', '\u041F',
            '\u0420', '\u0421', '\u0422', '\u0423', '\u0424', '\u0425', '\u0426', '\u0427',
            '\u0428', '\u0429', '\u042A', '\u042B', '\u042C', '\u042D', '\u042E', '\u042F',
            '\u0430', '\u0431', '\u0432', '\u0433', '\u0434', '\u0435', '\u0436', '\u0437',
            '\u0438', '\u0439', '\u043A', '\u043B', '\u043C', '\u043D', '\u043E', '\u043F',
            '\u0440', '\u0441', '\u0442', '\u0443', '\u0444', '\u0445', '\u0446', '\u0447',
            '\u0448', '\u0449', '\u044A', '\u044B', '\u044C', '\u044D', '\u044E', '\u044F',
            '\u0450', '\u0451', '\u0452', '\u0453', '\u0454', '\u0455', '\u0456', '\u0457',
            '\u0458', '\u0459', '\u045A', '\u045B', '\u045C', '\u045D', '\u045E', '\u045F',
            '\u0490', '\u0491', '\u05BE', '\u05D0', '\u05D1', '\u05D2', '\u05D3', '\u05D4',
            '\u05D5', '\u05D6', '\u05D7', '\u05D8', '\u05D9', '\u05DA', '\u05DB', '\u05DC',
            '\u05DD', '\u05DE', '\u05DF', '\u05E0', '\u05E1', '\u05E2', '\u05E3', '\u05E4',
            '\u05E5', '\u05E6', '\u05E7', '\u05E8', '\u05E9', '\u05EA', '\u05F0', '\u05F1',
            '\u05F2', '\u05F3', '\u05F4', '\u1D1B', '\u1D26', '\u1D28', '\u1E80', '\u1E81',
            '\u1E82', '\u1E83', '\u1E84', '\u1E85', '\u1E9F', '\u1EF2', '\u1EF3', '\u2010',
            '\u2012', '\u2013', '\u2014', '\u2015', '\u2017', '\u2018', '\u2019', '\u201A',
            '\u201B', '\u201C', '\u201D', '\u201E', '\u201F', '\u2020', '\u2021', '\u2022',
            '\u2026', '\u2027', '\u2030', '\u2032', '\u2033', '\u2035', '\u2039', '\u203A',
            '\u203C', '\u203E', '\u203F', '\u2040', '\u2044', '\u2054', '\u2074', '\u2075',
            '\u2076', '\u2077', '\u2078', '\u2079', '\u207A', '\u207B', '\u207F', '\u2081',
            '\u2082', '\u2083', '\u2084', '\u2085', '\u2086', '\u2087', '\u2088', '\u2089',
            '\u208A', '\u208B', '\u20A3', '\u20A4', '\u20A7', '\u20AA', '\u20AC', '\u2105',
            '\u2113', '\u2116', '\u2122', '\u2126', '\u212E', '\u2150', '\u2151', '\u2153',
            '\u2154', '\u2155', '\u2156', '\u2157', '\u2158', '\u2159', '\u215A', '\u215B',
            '\u215C', '\u215D', '\u215E', '\u2190', '\u2191', '\u2192', '\u2193', '\u2194',
            '\u2195', '\u21A8', '\u2202', '\u2205', '\u2206', '\u2208', '\u220F', '\u2211',
            '\u2212', '\u2215', '\u2219', '\u221A', '\u221E', '\u221F', '\u2229', '\u222B',
            '\u2248', '\u2260', '\u2261', '\u2264', '\u2265', '\u2299', '\u2300', '\u2302',
            '\u2310', '\u2320', '\u2321', '\u2500', '\u2502', '\u250C', '\u2510', '\u2514',
            '\u2518', '\u251C', '\u2524', '\u252C', '\u2534', '\u253C', '\u2550', '\u2551',
            '\u2552', '\u2553', '\u2554', '\u2555', '\u2556', '\u2557', '\u2558', '\u2559',
            '\u255A', '\u255B', '\u255C', '\u255D', '\u255E', '\u255F', '\u2560', '\u2561',
            '\u2562', '\u2563', '\u2564', '\u2565', '\u2566', '\u2567', '\u2568', '\u2569',
            '\u256A', '\u256B', '\u256C', '\u2580', '\u2581', '\u2584', '\u2588', '\u258C',
            '\u2590', '\u2591', '\u2592', '\u2593', '\u25A0', '\u25A1', '\u25AA', '\u25AB',
            '\u25AC', '\u25B2', '\u25BA', '\u25BC', '\u25C4', '\u25CA', '\u25CB', '\u25CF',
            '\u25D8', '\u25D9', '\u25E6', '\u263A', '\u263B', '\u263C', '\u2640', '\u2642',
            '\u2660', '\u2663', '\u2665', '\u2666', '\u266A', '\u266B', '\u2713', '\uFB01',
            '\uFB02', '\uFFFD'
        };

        internal static char[] BuildCodePage437()
            => Cp437.ToArray();

        internal static char[] BuildCodePage437Plus()
            => Cp437Plus.ToArray();
    }
}