import 'package:clinic_app/screens/admin_home_screen.dart';
import 'package:clinic_app/screens/change_password_screen.dart';
import 'package:clinic_app/screens/manage_users_screen.dart';
import 'package:clinic_app/screens/patients_screen.dart';
import 'package:clinic_app/screens/register_screen.dart';
import 'package:clinic_app/screens/user_home_screen.dart';
import 'package:flutter/material.dart';
import 'screens/login_screen.dart';

void main() {
  runApp(const MyApp());
}

class MyApp extends StatelessWidget {
  const MyApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      debugShowCheckedModeBanner: false,
      title: 'Clinic App',
      theme: ThemeData(useMaterial3: true, colorSchemeSeed: Colors.teal),
      initialRoute: '/',
      routes: {
        '/': (context) => const LoginScreen(),
        '/admin': (context) => const AdminHomeScreen(),
        '/user': (context) => const UserHomeScreen(),
        '/register': (context) => const RegisterScreen(),
        '/change-password': (context) => const ChangePasswordScreen(),
        '/patients': (context) => const PatientsScreen(),
        '/users-manage': (context) => const ManageUsersScreen(),
      },

      // (Tuỳ chọn) Hiển thị khi gõ sai route
      onUnknownRoute: (settings) => MaterialPageRoute(
        builder: (_) => Scaffold(
          appBar: AppBar(title: const Text('Route not found')),
          body: Center(child: Text('No route for ${settings.name}')),
        ),
      ),
    );
  }
}
