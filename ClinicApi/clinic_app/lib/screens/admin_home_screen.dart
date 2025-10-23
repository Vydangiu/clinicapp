import 'package:flutter/material.dart';
import '../services/auth_service.dart';

class AdminHomeScreen extends StatelessWidget {
  const AdminHomeScreen({super.key});

  @override
  Widget build(BuildContext context) {
    final auth = AuthService();

    return Scaffold(
      appBar: AppBar(
        title: const Text('Trang chủ (Admin)'),
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
      body: ListView(
        padding: const EdgeInsets.all(16),
        children: [
          FilledButton(
            onPressed: () => Navigator.pushNamed(context, '/register'),
            child: const Text('Tạo tài khoản (Admin)'),
          ),
          const SizedBox(height: 12),
          FilledButton.tonal(
            onPressed: () => Navigator.pushNamed(context, '/users-manage'),
            child: const Text('Quản lý người dùng'),
          ),
          const SizedBox(height: 12),
          FilledButton.tonal(
            onPressed: () => Navigator.pushNamed(context, '/patients'),
            child: const Text('Quản lý bệnh nhân'),
          ),
        ],
      ),
    );
  }
}
