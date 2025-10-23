import 'package:flutter/material.dart';
import '../services/auth_service.dart';

class UserHomeScreen extends StatelessWidget {
  const UserHomeScreen({super.key});

  @override
  Widget build(BuildContext context) {
    final auth = AuthService();

    return Scaffold(
      appBar: AppBar(
        title: const Text('Trang chủ (User)'),
        actions: [
          IconButton(
            tooltip: 'Đăng xuất',
            icon: const Icon(Icons.logout),
            onPressed: () async {
              await auth.logout();
              if (context.mounted) {
                Navigator.pushNamedAndRemoveUntil(context, '/', (_) => false);
              }
            },
          ),
        ],
      ),
      body: Center(
        child: FilledButton.tonal(
          onPressed: () => Navigator.pushNamed(context, '/patients'),
          child: const Text('Xem bệnh nhân'),
        ),
      ),
    );
  }
}
