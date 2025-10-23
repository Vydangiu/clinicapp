import 'package:flutter/material.dart';
import '../services/patient_service.dart';
import '../widgets/common_widgets.dart';

class PatientEditScreen extends StatefulWidget {
  const PatientEditScreen({super.key, required this.data});
  final Map<String, dynamic>? data;

  @override
  State<PatientEditScreen> createState() => _PatientEditScreenState();
}

class _PatientEditScreenState extends State<PatientEditScreen> {
  final _svc = PatientService();
  final _name = TextEditingController();
  final _phone = TextEditingController();
  final _email = TextEditingController();
  final _gender = TextEditingController();

  @override
  void initState() {
    super.initState();
    final d = widget.data;
    if (d != null) {
      _name.text = (d['fullName'] ?? d['FullName'] ?? '').toString();
      _phone.text = (d['phone'] ?? '').toString();
      _email.text = (d['email'] ?? '').toString();
      _gender.text = (d['gender'] ?? '').toString();
    }
  }

  @override
  void dispose() {
    _name.dispose();
    _phone.dispose();
    _email.dispose();
    _gender.dispose();
    super.dispose();
  }

  Future<void> _save() async {
    final body = {
      'fullName': _name.text.trim(),
      'phone': _phone.text.trim(),
      'email': _email.text.trim(),
      'gender': _gender.text.trim(),
    };

    try {
      Map<String, dynamic> result;
      if (widget.data == null) {
        result = await _svc.create(body);
      } else {
        final id = widget.data!['patientId'] ?? widget.data!['PatientID'];
        result = await _svc.update(id as int, {...widget.data!, ...body});
      }
      if (!mounted) return;
      Navigator.pop(context, result);
    } catch (e) {
      showError(context, e);
    }
  }

  @override
  Widget build(BuildContext context) {
    final isNew = widget.data == null;
    return Scaffold(
      appBar: AppBar(title: Text(isNew ? 'Thêm bệnh nhân' : 'Sửa bệnh nhân')),
      body: ListView(
        padding: const EdgeInsets.all(16),
        children: [
          TextField(
            decoration: const InputDecoration(labelText: 'Họ tên'),
            controller: _name,
          ),
          const SizedBox(height: 12),
          TextField(
            decoration: const InputDecoration(labelText: 'Điện thoại'),
            controller: _phone,
          ),
          const SizedBox(height: 12),
          TextField(
            decoration: const InputDecoration(labelText: 'Email'),
            controller: _email,
          ),
          const SizedBox(height: 12),
          TextField(
            decoration: const InputDecoration(labelText: 'Giới tính'),
            controller: _gender,
          ),
          const SizedBox(height: 16),
          FilledButton(onPressed: _save, child: const Text('Lưu')),
        ],
      ),
    );
  }
}
