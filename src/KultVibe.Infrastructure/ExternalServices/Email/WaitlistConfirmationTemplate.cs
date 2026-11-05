using System.Net;

namespace KultVibe.Infrastructure.ExternalServices.Email;

internal static class WaitlistConfirmationTemplate
{
    public static string Html(string name)
    {
        var safeName = WebUtility.HtmlEncode(name);

        // Match the frontend palette. Inline styles and presentation tables keep
        // the core layout intact in email clients that strip the responsive CSS.
        return $$"""
            <!doctype html>
            <html lang="en" xmlns="http://www.w3.org/1999/xhtml">
            <head>
              <meta charset="utf-8">
              <meta name="viewport" content="width=device-width, initial-scale=1">
              <meta name="color-scheme" content="dark">
              <meta name="supported-color-schemes" content="dark">
              <title>You're on the KultVibe waitlist</title>
              <style>
                body, table, td, p, h1, h2 { margin: 0; padding: 0; }
                table { border-collapse: collapse; mso-table-lspace: 0pt; mso-table-rspace: 0pt; }
                @media only screen and (max-width: 620px) {
                  .outer { padding: 16px 10px !important; }
                  .inset { padding-left: 24px !important; padding-right: 24px !important; }
                  .headline { font-size: 40px !important; line-height: 44px !important; }
                  .brand { font-size: 23px !important; }
                  .edition { font-size: 9px !important; letter-spacing: 1px !important; }
                }
              </style>
              <!--[if mso]><style>body, table, td, h1, h2, p { font-family: Arial, sans-serif !important; }</style><![endif]-->
            </head>
            <body style="margin:0; padding:0; width:100%; background-color:#08080D; color:#FFFFFF; -webkit-text-size-adjust:100%; -ms-text-size-adjust:100%;">
              <div style="display:none; font-size:1px; line-height:1px; color:#08080D; max-height:0; max-width:0; overflow:hidden; mso-hide:all;" aria-hidden="true">You're on the list. We'll email you when your KultVibe access opens.</div>
              <table role="presentation" width="100%" bgcolor="#08080D" style="width:100%; background-color:#08080D;">
                <tr>
                  <td class="outer" align="center" style="padding:40px 16px;">
                    <!--[if mso]><table role="presentation" width="600" align="center"><tr><td><![endif]-->
                    <table role="presentation" width="100%" align="center" bgcolor="#0D0D14" style="width:100%; max-width:600px; background-color:#0D0D14; border:1px solid #252530; font-family:Arial, Helvetica, sans-serif;">
                      <tr>
                        <td style="height:5px; background-color:#F0B429; font-size:0; line-height:0;" bgcolor="#F0B429">&nbsp;</td>
                      </tr>
                      <tr>
                        <td class="inset" style="padding:30px 40px; border-bottom:1px solid #252530;">
                          <table role="presentation" width="100%" style="width:100%;">
                            <tr>
                              <td class="brand" style="font-size:28px; line-height:34px; font-weight:800; letter-spacing:-1px; color:#FFFFFF;">Kult<span style="color:#F0B429;">Vibe</span><span style="color:#F0B429;">.</span></td>
                              <td class="edition" align="right" style="font-family:'Courier New', monospace; font-size:10px; line-height:16px; letter-spacing:2px; color:#B4B4C2;">THE NEXT WAVE<br><span style="color:#F0B429;">STARTS HERE</span></td>
                            </tr>
                          </table>
                        </td>
                      </tr>
                      <tr>
                        <td class="inset" style="padding:36px 40px 0;">
                          <p style="margin:0 0 24px; font-family:'Courier New', monospace; font-size:11px; line-height:18px; font-weight:700; letter-spacing:2px; color:#10B981;">&#10003; &nbsp; WAITLIST CONFIRMED</p>
                          <h1 class="headline" style="margin:0; font-size:54px; line-height:58px; letter-spacing:-2px; font-weight:800; color:#FFFFFF;">Your Culture.<br>Your Stage.<br><span style="color:#F0B429;">Your Money.</span></h1>
                          <p style="margin:24px 0 0; font-size:16px; line-height:26px; color:#B4B4C2;">Bring your game. Bring your sound.<br>Bring your whole vibe.</p>
                        </td>
                      </tr>
                      <tr>
                        <td class="inset" style="padding:32px 40px 0;">
                          <table role="presentation" width="100%" bgcolor="#111118" style="width:100%; background-color:#111118; border:1px solid #252530;">
                            <tr>
                              <td style="padding:24px; border-left:3px solid #F0B429; overflow-wrap:anywhere; word-break:break-word;">
                                <h2 style="margin:0 0 16px; font-size:24px; line-height:30px; font-weight:700; color:#FFFFFF;">You're on the list.</h2>
                                <p style="margin:0 0 12px; font-size:15px; line-height:25px; color:#FFFFFF;">Hey {{safeName}},</p>
                                <p style="margin:0 0 16px; font-size:15px; line-height:25px; color:#B4B4C2;">Your spot on the KultVibe waitlist is confirmed. A new stage for Africa's gamers, creators, and culture is taking shape &mdash; and you're part of the next wave.</p>
                                <p style="margin:0; font-size:15px; line-height:25px; color:#FFFFFF;">We'll email you as soon as your access opens.<br><span style="color:#B4B4C2;">No extra steps. Just stay ready.</span></p>
                              </td>
                            </tr>
                          </table>
                        </td>
                      </tr>
                      <tr>
                        <td class="inset" style="padding:28px 40px 32px;">
                          <p style="margin:0 0 8px; font-size:14px; line-height:22px; color:#B4B4C2;">See you in the Kult.</p>
                          <p style="margin:0; font-size:14px; line-height:22px; font-weight:700; color:#F0B429;">The KultVibe Team</p>
                        </td>
                      </tr>
                      <tr>
                        <td class="inset" bgcolor="#111118" style="padding:22px 40px; border-top:1px solid #252530; background-color:#111118;">
                          <p style="margin:0 0 12px; font-size:11px; line-height:20px; font-weight:700; letter-spacing:1px;"><span style="color:#F0B429;">COMPETE.</span> &nbsp; <span style="color:#DB2777;">STREAM.</span> &nbsp; <span style="color:#10B981;">EARN.</span></p>
                          <p style="margin:0; font-size:11px; line-height:18px; color:#A2A2B2;">You received this confirmation because you joined the KultVibe waitlist. If that wasn't you, you can ignore this email.</p>
                        </td>
                      </tr>
                    </table>
                    <!--[if mso]></td></tr></table><![endif]-->
                  </td>
                </tr>
              </table>
            </body>
            </html>
            """;
    }

    public static string Text(string name) => $"""
        KULTVIBE
        Your Culture. Your Stage. Your Money.

        WAITLIST CONFIRMED

        Hey {name},

        You're on the list. Your spot on the KultVibe waitlist is confirmed.
        A new stage for Africa's gamers, creators, and culture is taking shape — and you're part of the next wave.

        Bring your game. Bring your sound. Bring your whole vibe.
        We'll email you as soon as your access opens. No extra steps. Just stay ready.

        See you in the Kult.
        The KultVibe Team

        COMPETE. STREAM. EARN.

        You received this confirmation because you joined the KultVibe waitlist.
        If that wasn't you, you can ignore this email.
        """;
}
