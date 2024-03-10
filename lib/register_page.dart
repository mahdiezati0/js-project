import 'package:flutter/material.dart';
import 'package:note_app/login_page.dart';
import 'package:http/http.dart' as http;

class RegisterPage extends StatelessWidget {
  RegisterPage({super.key});
  TextEditingController controllerEmail = TextEditingController();
  TextEditingController controllerPassword = TextEditingController();
  TextEditingController controllerName = TextEditingController();
  TextEditingController controllerConfirm = TextEditingController();

  @override
  Widget build(BuildContext context) {
    TextTheme textTheme = Theme.of(context).textTheme;
    final formkey = GlobalKey<FormState>();
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
                  mainAxisAlignment: MainAxisAlignment.center,
                  children: [
                    Column(
                      children: [
                        Image.asset("assets/images/Register.png"),
                        SizedBox(height: 19),
                        Row(
                          children: [
                            Text(
                              "Register",
                              style: textTheme.bodyMedium!.copyWith(
                                fontSize: 32,
                              ),
                            ),
                          ],
                        )
                      ],
                    ),
                    SizedBox(
                      height: 19,
                    ),
                    Form(
                      key: formkey,
                      child: Column(
                        children: [
                          TextFormField(
                            controller: controllerName,
                            decoration: InputDecoration(hintText: "Name"),
                            validator: (value) {
                              if (value!.isEmpty) {
                                return "Please enter a name";
                              }
                            },
                          ),
                          SizedBox(height: 15),
                          TextFormField(
                            controller: controllerEmail,
                            decoration:
                                InputDecoration(hintText: "Email Address"),
                            validator: (value) => validateEmail(value),
                          ),
                          SizedBox(height: 15),
                          TextFormField(
                            controller: controllerPassword,
                            decoration: InputDecoration(hintText: "Password"),
                            validator: (value) => validatePassword(value),
                          ),
                          SizedBox(height: 15),
                          TextFormField(
                            controller: controllerConfirm,
                            decoration:
                                InputDecoration(hintText: "Confirm Password"),
                            validator: (value) {
                              if (value!.isEmpty) {
                                return "Please confirm your password";
                              }
                              if (value != controllerPassword.text) {
                                return "Passwords do not match";
                              }
                            },
                          ),
                        ],
                      ),
                    ),
                    SizedBox(
                      height: 16,
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
                        onPressed: () {
                          formkey.currentState!.validate();
                        },
                        child: Text(
                          'Sign Up',
                          style: textTheme.bodyMedium!.copyWith(
                            fontSize: 25,
                          ),
                        ),
                      ),
                    ),
                    SizedBox(
                      height: 11,
                    ),
                    Row(
                      mainAxisAlignment: MainAxisAlignment.center,
                      children: [
                        TextButton(
                          onPressed: () => Navigator.push(
                              context,
                              MaterialPageRoute(
                                  builder: (context) => LoginPage())),
                          child: Text(
                            "Already Have An Account?",
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
                      height: 58,
                    )
                  ],
                ),
              )),
        ),
      ),
    );
  }

  String? validateEmail(String? value) {
    const pattern = r"(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'"
        r'*+/=?^_`{|}~-]+)*|"(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-'
        r'\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*")@(?:(?:[a-z0-9](?:[a-z0-9-]*'
        r'[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:(2(5[0-5]|[0-4]'
        r'[0-9])|1[0-9][0-9]|[1-9]?[0-9]))\.){3}(?:(2(5[0-5]|[0-4][0-9])|1[0-9]'
        r'[0-9]|[1-9]?[0-9])|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\'
        r'x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])';
    final regex = RegExp(pattern);
    if (value!.isEmpty) {
      return 'Please enter an email';
    } else {
      return value!.isNotEmpty && !regex.hasMatch(value)
          ? 'Enter a valid email address'
          : null;
    }
  }

  String? validatePassword(String? value) {
    RegExp regex =
        RegExp(r'^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[!@#\$&*~]).{8,}$');
    if (value!.isEmpty) {
      return 'Please enter a password';
    } else if (value.length < 8) {
      return 'Must be at least 8 characters in length';
    } else {
      if (!regex.hasMatch(value)) {
        return 'Enter a valid password';
      } else {
        return null;
      }
    }
  }

  Future sendDate() async {
    var url =
        Uri.parse("https://mahdiezati0-js-project.liara.run/Account/Register/");
    Map<String, String> body = <String, String>{
      "name": controllerName.text,
      "email": controllerEmail.text,
      "password": controllerPassword.text,
    };
    var response = await http.post(url, body: body);
    debugPrint(response.statusCode.toString());
    debugPrint(response.body);
  }
}
