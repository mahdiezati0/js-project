import 'package:flutter/material.dart';
import 'package:note_app/forgot_password.dart';
import 'package:note_app/register_page.dart';

class LoginPage extends StatelessWidget {
  const LoginPage({super.key});

  @override
  Widget build(BuildContext context) {
    TextTheme textTheme = Theme.of(context).textTheme;
    return Scaffold(
      appBar: AppBar(
        // title: Text('back', style: Colors.white,),
        backgroundColor: Color(0xff222831),
        elevation: 0.0,
        leading: IconButton(
          icon: Icon(Icons.arrow_back_ios),
          // padding: EdgeInsets.zero,
          // visualDensity: VisualDensity(horizontal: -4.0, vertical: -4.0),
          color: Color(0xffEEEEEE),
          onPressed: () {
            Navigator.of(context).pop();
          },
        ),
        // title: Text(
        //   'Back',
        //   style: TextStyle(color: Colors.white),
        //   textAlign: TextAlign.left,
        // ),
      ),
      body: SafeArea(
        child: Center(
          child: SingleChildScrollView(
              physics: const BouncingScrollPhysics(),
              child: Padding(
                padding: EdgeInsets.all(20),
                child: Column(
                  mainAxisAlignment: MainAxisAlignment.start,
                  children: [
                    Column(
                      children: [
                        Text(
                          "WELCOME BACK",
                          style: TextStyle(
                            fontWeight: FontWeight.normal,
                            fontSize: 24,
                          ),
                        ),
                        SizedBox(
                          height: 27,
                        ),
                        Image.asset("assets/images/login.png"),
                        SizedBox(height: 20),
                        Row(
                          children: [
                            Text(
                              "Sign in",
                              style: textTheme.bodyMedium!.copyWith(
                                fontSize: 32,
                              ),
                            ),
                          ],
                        )
                      ],
                    ),
                    SizedBox(
                      height: 20,
                    ),
                    Form(
                      child: Column(
                        children: [
                          TextField(
                            decoration:
                                InputDecoration(hintText: "email address"),
                          ),
                          SizedBox(height: 15),
                          TextField(
                            decoration: InputDecoration(hintText: "password"),
                          ),
                        ],
                      ),
                    ),
                    SizedBox(height: 6),
                    Row(
                      mainAxisAlignment: MainAxisAlignment.end,
                      children: [
                        TextButton(
                          onPressed: () => Navigator.push(
                              context,
                              MaterialPageRoute(
                                  builder: (context) => ForgotPassword())),
                          child: Text(
                            "I Forgot My Password.",
                            style: TextStyle(
                              fontFamily: "roboto",
                              color: Colors.white,
                              fontWeight: FontWeight.w500,
                              decoration: TextDecoration.underline,
                              decorationColor: Colors.white,
                            ),
                            textAlign: TextAlign.left,
                          ),
                          style: ButtonStyle(
                              backgroundColor:
                                  MaterialStateProperty.all(Colors.transparent),
                              shape: MaterialStateProperty.all(
                                  null) // Transparent background
                              ),
                        )
                      ],
                    ),
                    SizedBox(
                      height: 26,
                    ),
                    SizedBox(
                      width: 340,
                      height: 55,
                      child: TextButton(
                        style: Theme.of(context)
                            .textButtonTheme
                            .style!
                            .copyWith(
                              backgroundColor:
                                  MaterialStateProperty.all(Color(0xff00ADB5)),
                              shadowColor:
                                  MaterialStateProperty.all(Colors.black),
                              elevation: MaterialStateProperty.all(20),
                            ),
                        onPressed: null,
                        child: Text(
                          'Sign in',
                          style: textTheme.bodyMedium!.copyWith(
                            fontSize: 25,
                          ),
                        ),
                      ),
                    ),
                    SizedBox(
                      height: 14,
                    ),
                    Row(
                      mainAxisAlignment: MainAxisAlignment.center,
                      children: [
                        TextButton(
                          onPressed: () => Navigator.push(
                              context,
                              MaterialPageRoute(
                                  builder: (context) => RegisterPage())),
                          child: Text(
                            "I Don't Have An Account Yet.",
                            style: TextStyle(
                              color: Colors.white,
                              fontFamily: "roboto",
                              fontWeight: FontWeight.normal,
                              fontSize: 10,
                              decoration: TextDecoration.underline,
                              decorationColor: Colors.white,
                            ),
                          ),
                          style: ButtonStyle(
                              backgroundColor:
                                  MaterialStateProperty.all(Colors.transparent),
                              shape: MaterialStateProperty.all(
                                  null) // Transparent background
                              ),
                        ),
                      ],
                    ),
                    SizedBox(
                      height: 64,
                    )
                  ],
                ),
              )),
        ),
      ),
    );
  }
}
