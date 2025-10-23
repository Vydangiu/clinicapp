import 'package:flutter/material.dart';
import '../services/auth_service.dart';

class AdminResetScreen extends StatefulWidget {
  const AdminResetScreen({super.key});

  @override
  State<AdminResetScreen> createState() => _AdminResetScreenState();
}

class _AdminResetScreenState extends State<AdminResetScreen> {
  final _id = TextEditingController();
  final _new = TextEditingController();
  bool _see = true;
  bool _loading = false;
  final _auth = AuthService();

  Future<void> _submit() async {
    setState(() => _loading = true);
    try {
      await _auth.adminResetPassword(int.parse(_id.text), _new.text);
      if (!mounted) return;
      ScaffoldMessenger.of(
        context,
      ).showSnackBar(const SnackBar(content: Text('Reset OK')));
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
      appBar: AppBar(title: const Text('Admin reset mật khẩu')),
      body: Padding(
        padding: const EdgeInsets.all(16),
        child: ListView(
          children: [
            TextField(
              controller: _id,
              keyboardType: TextInputType.number,
              decoration: const InputDecoration(labelText: 'User ID'),
            ),
            const SizedBox(height: 8),
            TextField(
              controller: _new,
              obscureText: _see,
              decoration: InputDecoration(
                labelText: 'Mật khẩu mới',
                suffixIcon: IconButton(
                  icon: Icon(_see ? Icons.visibility : Icons.visibility_off),
                  onPressed: () => setState(() => _see = !_see),
                ),
              ),
            ),
            const SizedBox(height: 16),
            FilledButton(
              onPressed: _loading ? null : _submit,
              child: _loading
                  ? const CircularProgressIndicator()
                  : const Text('Đặt lại'),
            ),
          ],
        ),
      ),
    );
  }
}
