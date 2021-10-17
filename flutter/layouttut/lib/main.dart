// ignore_for_file: prefer_const_constructors

import 'package:flutter/material.dart';
import 'package:layouttut/page1/PageOne.dart';

void main(){
  runApp(MyApp());
}

class MyApp extends StatelessWidget {
  const MyApp({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'My App',
      theme: ThemeData(
        primarySwatch: Colors.red,
      ),
      home: PageOne(),
    );
  }
}

