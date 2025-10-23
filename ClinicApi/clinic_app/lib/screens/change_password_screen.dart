import 'package:flutter/material.dart';
import '../services/auth_service.dart';

class ChangePasswordScreen extends StatefulWidget {
  const ChangePasswordScreen({super.key});

  @override
  State<ChangePasswordScreen> createState() => _ChangePasswordScreenState();
}

class _ChangePasswordScreenState extends State<ChangePasswordScreen> {
  final _old = TextEditingController();
  final _new = TextEditingController();
  bool _o1 = true;
  bool _o2 = true;
  bool _loading = false;
  final _auth = AuthService();

  Future<void> _submit() async {
    setState(() => _loading = true);
    try {
      await _auth.changePassword(_old.text, _new.text);
      if (!mounted) return;
      ScaffoldMessenger.of(
        context,
      ).showSnackBar(const SnackBar(content: Text('Đổi mật khẩu OK')));
    } catch (e) {
      if (mounted) {
        ScaffoldMessenger.of(
          context,
        ).showSnackBar(SnackBar(content: Text('Lỗi: $e')));
      }
    } finally {
      if (mounted) setState(() => _loading = false);
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: const Text('Đổi mật khẩu')),
      body: Padding(
        padding: const EdgeInsets.all(16),
        child: ListView(
          children: [
            TextField(
              controller: _old,
              obscureText: _o1,
              decoration: InputDecoration(
                labelText: 'Mật khẩu cũ',
                suffixIcon: IconButton(
                  icon: Icon(_o1 ? Icons.visibility : Icons.visibility_off),
                  onPressed: () => setState(() => _o1 = !_o1),
                ),
              ),
            ),
            const SizedBox(height: 8),
            TextField(
              controller: _new,
              obscureText: _o2,
              decoration: InputDecoration(
                labelText: 'Mật khẩu mới',
                suffixIcon: IconButton(
                  icon: Icon(_o2 ? Icons.visibility : Icons.visibility_off),
                  onPressed: () => setState(() => _o2 = !_o2),
                ),
              ),
            ),
            const SizedBox(height: 16),
            FilledButton(
              onPressed: _loading ? null : _submit,
              child: _loading
                  ? const CircularProgressIndicator()
                  : const Text('Đổi mật khẩu'),
            ),
          ],
        ),
      ),
    );
  }
}
