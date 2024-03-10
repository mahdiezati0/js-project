import 'package:flutter/material.dart';
import 'package:note_app/set_password.dart';

class AuthenticationPage extends StatelessWidget {
  const AuthenticationPage({super.key});

  @override
  Widget build(BuildContext context) {
    TextTheme textTheme = Theme.of(context).textTheme;
    return Scaffold(
      appBar: AppBar(
        backgroundColor: Color(0xff222831),
        elevation: 0.0,
        leading: IconButton(
          icon: Icon(Icons.arrow_back_ios),
          color: Color(0xffEEEEEE),
          onPressed: () {
            Navigator.of(context).pop();
          },
        ),
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
                        Image.asset("assets/images/Authentication.png"),
                        SizedBox(height: 32),
                        Text(
                          "Email",
                          style: textTheme.bodyMedium!.copyWith(
                            height: 0,
                            fontSize: 36,
                            fontFamily: 'Inter',
                          ),
                        ),
                        Text(
                          "Authentication",
                          style: textTheme.bodyMedium!.copyWith(
                            height: 0,
                            fontSize: 40,
                            fontFamily: 'Inter',
                            color: Color(0xff00ADB5),
                          ),
                        ),
                      ],
                    ),
                    SizedBox(
                      height: 12,
                    ),
                    Row(
                      children: [
                        Text(
                          "Enter your 5-digit authentication code: ",
                          style: textTheme.bodyMedium!.copyWith(
                            fontSize: 13,
                            fontWeight: FontWeight.bold,
                            fontFamily: 'Inter',
                          ),
                        ),
                      ],
                    ),
                    SizedBox(
                      height: 11,
                    ),
                    Form(
                      child: Column(
                        children: [],
                      ),
                    ),
                    SizedBox(
                      height: 50,
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
                        onPressed: () => Navigator.push(
                            context,
                            MaterialPageRoute(
                                builder: (context) => SetPassword())),
                        child: Text(
                          'Verify',
                          style: textTheme.bodyMedium!.copyWith(
                            fontSize: 23,
                          ),
                        ),
                      ),
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
